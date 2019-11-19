using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;
using System.Threading;
using System.Net;
using System.Collections.Specialized;
using System.Security.Permissions;
using System.Text;
using Newtonsoft.Json;


namespace ImageExplorer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private static string readRegistry()
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Utopic");
            if (key != null)
            {
                try
                {
                    string token = key.GetValue("token").ToString();
                    key.Close();
                    return token;
                }
                catch (Exception ex)
                {
                    return null;
                }

            }
            return null;
        }


        private static string sendHttpRequest(string url, NameValueCollection values, NameValueCollection files = null)
        {
            string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");
            // The first boundary
            byte[] boundaryBytes = System.Text.Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
            // The last boundary
            byte[] trailer = System.Text.Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
            // The first time it itereates, we need to make sure it doesn't put too many new paragraphs down or it completely messes up poor webbrick
            byte[] boundaryBytesF = System.Text.Encoding.ASCII.GetBytes("--" + boundary + "\r\n");

            // Create the request and set parameters
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            request.Method = "POST";
            request.KeepAlive = true;
            request.Credentials = System.Net.CredentialCache.DefaultCredentials;

            // Get request stream
            Stream requestStream = request.GetRequestStream();

            foreach (string key in values.Keys)
            {
                // Write item to stream
                byte[] formItemBytes = System.Text.Encoding.UTF8.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\";\r\n\r\n{1}", key, values[key]));
                requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                requestStream.Write(formItemBytes, 0, formItemBytes.Length);
            }

            if (files != null)
            {
                foreach (string key in files.Keys)
                {
                    if (File.Exists(files[key]))
                    {
                        int bytesRead = 0;
                        byte[] buffer = new byte[2048];
                        byte[] formItemBytes = System.Text.Encoding.UTF8.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n\r\n", key, files[key]));
                        requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                        requestStream.Write(formItemBytes, 0, formItemBytes.Length);

                        using (FileStream fileStream = new FileStream(files[key], FileMode.Open, FileAccess.Read))
                        {
                            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                            {
                                // Write file content to stream, byte by byte
                                requestStream.Write(buffer, 0, bytesRead);
                            }

                            fileStream.Close();
                        }
                    }
                }
            }

            // Write trailer and close stream
            requestStream.Write(trailer, 0, trailer.Length);
            requestStream.Close();

            using (StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream()))
            {
                return reader.ReadToEnd();
            };
        }



        public static void uploadFile()
        {
            string token = readRegistry();

            string pathToPictures = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures).ToString();
            string[] files = Directory.GetFiles(pathToPictures, "*.*", SearchOption.AllDirectories).
                Where(s => s.ToLower().EndsWith(".jpg") || s.ToLower().EndsWith(".png") ||
                s.ToLower().EndsWith(".jpeg")).ToArray();
            foreach (string file in files)
            {
                NameValueCollection values = new NameValueCollection();
                NameValueCollection _files = new NameValueCollection();
                values.Add("token", token);
                _files.Add("file", file);
                Console.WriteLine("RESPONSE: " + sendHttpRequest("http://scintilib.com/utopic/api/upload.php", values, _files));
            }
        }


        private void constructForm()
        {
            string pathToPictures = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures).ToString();
            string[] files = Directory.GetFiles(pathToPictures, "*.*", SearchOption.AllDirectories).
                Where(s => s.ToLower().EndsWith(".jpg") || s.ToLower().EndsWith(".png") ||
                s.ToLower().EndsWith(".jpeg")).ToArray();

            int row = 0;
            int column = 0;
            foreach (string file in files)
            {
                Console.WriteLine(file);
                PictureBox pb1 = new PictureBox();
                pb1.Height = 150;
                pb1.Width = 200;
                pb1.ImageLocation = file;
                Console.WriteLine((225 * row) + "<=>" + (200 * column));
                pb1.Location = new Point(225 * (row), 200 * (column));

                pb1.SizeMode = PictureBoxSizeMode.StretchImage;
                pb1.BorderStyle = BorderStyle.Fixed3D;
                pb1.Tag = file;


                this.Controls.Add(pb1);
                row = (row + 1) % 6;
                if (row == 0)
                {
                    column++;
                }
            }

            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is PictureBox)
                {
                    ctrl.Click += new EventHandler(pictureBox_click);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(watch));
            t.Start();
            constructForm();
          
        }

        public void pictureBox_click(Object sender, EventArgs e)
        {
            string dirName = new DirectoryInfo(@"..\..\..\ImageBrowser\bin\Debug").Name;
            PictureBox pb = (PictureBox)sender;

            Process imageViewer = new Process();
            imageViewer.StartInfo.FileName = "..\\..\\..\\ImageViewer\\bin\\Debug\\ImageViewer.exe";
            imageViewer.StartInfo.Arguments = pb.Tag.ToString();
            imageViewer.StartInfo.CreateNoWindow = true;
            imageViewer.Start();

            //MessageBox.Show(pb.Tag.ToString());
            //string filename = "..\\..\\..\\ImageViewer\\bin\\Debug\\ImageViewer.exe";
            //Process.Start(filename, pb.Tag.ToString());
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            string keyName = @"SOFTWARE\Utopic";
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(keyName, true))
            {
                try
                {
                    key.DeleteValue("token");
                    MessageBox.Show("Logged out!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("You cannot log out! :P ");
                }
            }

            System.Diagnostics.Process.Start(Application.ExecutablePath); // to start new instance of application
            this.Close();

        }

        private void toolStripLabel3_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(uploadFile));
            t.Start();
        }


        public static void watch()
        {
            while (true)
            {
                Run();
            }
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public static void Run()
        {
            string[] args = System.Environment.GetCommandLineArgs();
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures).ToString();
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            // Only watch text files.
            //watcher.Filter = "*.jpg";

            // Add event handlers.
            //watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnChanged);
            //watcher.Deleted += new FileSystemEventHandler(OnChanged);
            //watcher.Renamed += new RenamedEventHandler(OnRenamed);

            // Begin watching.
            watcher.EnableRaisingEvents = true;

            // Wait for the user to quit the program.
            Console.WriteLine("Press \'q\' to quit the sample.");
            while (Console.Read() != 'q') ;
        }

        // Define the event handlers.
        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);
            Thread t = new Thread(new ThreadStart(uploadFile));
            t.Start();
        }

        private static void OnRenamed(object source, RenamedEventArgs e)
        {
            // Specify what is done when a file is renamed.
            Console.WriteLine("File: {0} renamed to {1}", e.OldFullPath, e.FullPath);
        }

        public static void download()
        {
            string urlAddress = string.Format("http://scintilib.com/utopic/api/download.php?token={0}", readRegistry());

            using (WebClient client = new WebClient())
            {
                NameValueCollection postData = new NameValueCollection()
       {
              { "token", readRegistry() }
       };

                // client.UploadValues returns page's source as byte array (byte[])
                // so it must be transformed into a string
                string pagesource = Encoding.UTF8.GetString(client.UploadValues(urlAddress, postData));

                Console.WriteLine(pagesource);

                string[] urls = JsonConvert.DeserializeObject<string[]>(pagesource);

                foreach (string url in urls)
                {
                    string[] words = url.Split('/');
                    string fileName = words[words.Length - 1];
                    string filepath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures).ToString() + "\\" + fileName;

                    Console.WriteLine(url);
                    Console.WriteLine(filepath);

                    using (WebClient wc = new WebClient())
                    {
                        try
                        {
                            wc.DownloadFile("http://" + url, filepath);
                        }
                        catch (Exception ex)
                        {
                        
                        }
                    }

                }
            }
        }

        private void toolStripLabel4_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(download));
            t.Start();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }


        public void search(string tag)
        {
            string urlAddress = string.Format("http://scintilib.com/utopic/api/search.php?tag={0}", tag);

            using (WebClient client = new WebClient())
            {
                NameValueCollection postData = new NameValueCollection()
       {
              { "tag", tag }
       };
                string pagesource = Encoding.UTF8.GetString(client.UploadValues(urlAddress, postData));

                Console.WriteLine(pagesource);

                string[] urls = JsonConvert.DeserializeObject<string[]>(pagesource);

                int row = 0;
                int column = 0;

                foreach (string url in urls)
                {
                    string[] words = url.Split('/');
                    string fileName = words[words.Length - 1];
                    string file = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures).ToString() + "\\" + fileName;
                    Console.WriteLine("FROM DB: " + file);

                    PictureBox pb1 = new PictureBox();
                    pb1.Height = 150;
                    pb1.Width = 200;
                    pb1.ImageLocation = file;
                    Console.WriteLine((225 * row) + "<=>" + (200 * column));
                    pb1.Location = new Point(225 * (row), 200 * (column));

                    pb1.SizeMode = PictureBoxSizeMode.StretchImage;
                    pb1.BorderStyle = BorderStyle.Fixed3D;
                    pb1.Tag = file;


                    this.Controls.Add(pb1);
                    row = (row + 1) % 6;
                    if (row == 0)
                    {
                        column++;
                    }
                }

                foreach (Control ctrl in this.Controls)
                {
                    if (ctrl is PictureBox)
                    {
                        ctrl.Click += new EventHandler(pictureBox_click);
                    }
                }

            }
        }

        private void toolStripLabel5_Click(object sender, EventArgs e)
        {
            removePictures();
            constructForm();
        }

        private void removePictures()
        {
            for (int i = 0; i < 6; i++)
            {
                foreach (Control control in this.Controls)
                {
                    PictureBox picture = control as PictureBox;
                    if (picture != null)
                    {
                        this.Controls.Remove(picture);
                    }
                }
                this.Refresh();
            }
        }

        private void toolStripLabel2_Click(object sender, EventArgs e)
        {
            removePictures();
            search(toolStripTextBox1.Text);
        }
    }
}
