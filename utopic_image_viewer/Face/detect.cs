using System.IO;
using System;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;
using System.Web;

namespace Face
{
    public class detect
    {
        private static string subsKey = "c5b4f292e82b4e8c87652ef718b07843";

        public static async void MakeFaceRequestUri(string imageFilePath)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subsKey);

            // Request parameters
            queryString["returnFaceId"] = "true";
            //queryString["returnFaceLandmarks"] = "true";
            queryString["returnFaceAttributes"] = "age,gender,emotion,accessories";
            var uri = "https://southeastasia.api.cognitive.microsoft.com/face/v1.0/detect?" + queryString;

            HttpResponseMessage response;
            string responseContent;
            
            //Request body
            string body = "{ \"url\":\"" + imageFilePath + "\"}";
            byte[] byteData = Encoding.UTF8.GetBytes(body);

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = await client.PostAsync(uri, content);
                responseContent = response.Content.ReadAsStringAsync().Result;
            }
            Console.WriteLine(responseContent);
        }

        static byte[] GetImageFile(string imageFilePath)
        {
            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }

        public static async void MakeFaceRequestFile(string imageFilePath)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subsKey);

            // Request parameters
            queryString["returnFaceId"] = "true";
            //queryString["returnFaceLandmarks"] = "true";
            queryString["returnFaceAttributes"] = "age,gender,emotion,accessories";
            var uri = "https://southeastasia.api.cognitive.microsoft.com/face/v1.0/detect?" + queryString;

            HttpResponseMessage response;
            string responseContent;
            
            // Request body
            byte[] byteData = GetImageFile(imageFilePath);

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = await client.PostAsync(uri, content);
                responseContent = response.Content.ReadAsStringAsync().Result;
            }
            Console.WriteLine(responseContent);
        }
    }
}
