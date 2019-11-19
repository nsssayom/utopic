using System;
using System.Net;
using Newtonsoft.Json;
using System.Windows;
using Microsoft.Win32;

namespace Login_Registration
{
    class Login
    {
        public static Boolean login(string username, string password)
        {
            string URI = "http://scintilib.com/utopic/api/login.php";
            string myParameters = string.Format("user_login={0}&password={1}", username, password);

            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                string HtmlResult = wc.UploadString(URI, myParameters);
                Console.WriteLine(HtmlResult.ToString());

                dynamic response = JsonConvert.DeserializeObject(HtmlResult.ToString());

                var status = response[0].status;
                string token = (string)response[0].token;
                if (status == "100")
                {
                    MessageBox.Show("Login Successful!", "Success",MessageBoxButton.OK, MessageBoxImage.Information);
                    Login.writeRegistry(token);
                    return true;
                }
                else
                {
                    MessageBox.Show("Invalid Username or Password!", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                return false; 
            }
        }

        public static void writeRegistry(string token)
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Utopic");
                key.SetValue("token", token);
                key.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static string readRegistry()
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
                catch(Exception ex)
                {
                    return null;
                }
                
            }
            return null;
        }

        public static Boolean verifyToken(string token)
        {
            string URI = "http://scintilib.com/utopic/api/verify_token.php";
            string myParameters = string.Format("token={0}", token);

            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                string HtmlResult = wc.UploadString(URI, myParameters);
                Console.WriteLine(HtmlResult.ToString());

                dynamic response = JsonConvert.DeserializeObject(HtmlResult.ToString());

                var status = response[0].status;
                
                if (status == "100")
                {
                    return true;
                    //MessageBox.Show("Token Found and verified!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            return false;
        }
    }
}
