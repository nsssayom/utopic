using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using System.Windows;

namespace Login_Registration
{
    class Register
    {
        public static Boolean register(string username, string password, string phone, string email)
        {
            string URI = "http://scintilib.com/utopic/api/register.php";
            string myParameters = string.Format("username={0}&password={1}&phone={2}&email={3}", username, password, phone, email);

            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                string HtmlResult = wc.UploadString(URI, myParameters);
                Console.WriteLine(HtmlResult.ToString());

                dynamic response = JsonConvert.DeserializeObject(HtmlResult.ToString());

                var status = response[0].status;
                if (status == "100")
                {
                    MessageBox.Show("Registration Completed!", "Success",MessageBoxButton.OK, MessageBoxImage.Information);
                    return true;
                }
                else if (status == "301")
                {
                    MessageBox.Show("Username Not Available!", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (status == "302")
                {
                    MessageBox.Show("Invalid Email Address!", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (status == "303")
                {
                    MessageBox.Show("Email Already Used!", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (status == "304")
                {
                    MessageBox.Show("Invalid Phone Number", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (status == "305")
                {
                    MessageBox.Show("Phone Number Already Used!", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return false;
        }
    }
}
