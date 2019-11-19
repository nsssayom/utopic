using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fileUploadTest
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Net.WebClient Client = new System.Net.WebClient();
            Client.Headers.Add("Content-Type", "binary/octet-stream");
            byte[] result = Client.UploadFile("http://localhost/utopic/upload.php", "POST", @"C:\Users\Sayom\Desktop\face_suggestion\image.jpg");
            string s = System.Text.Encoding.UTF8.GetString(result, 0, result.Length);
        }
    }
}
