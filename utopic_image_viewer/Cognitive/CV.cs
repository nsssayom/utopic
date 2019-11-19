using System.IO;
using System;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;
using System.Web;


namespace Cognitive
{
    public class CV
    {
        private static string subsKey = "dd959986b2184ce59a4a8a90a8cab17c";

        public static async void MakeCVRequestUri(string imageFilePath)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subsKey);

            // Request parameters
            queryString["visualFeatures"] = "Categories,Tags,Description,Faces,ImageType,Color,Adult";
            queryString["details"] = "Celebrities,Landmarks";
            queryString["language"] = "en";
            var uri = "https://southeastasia.api.cognitive.microsoft.com/vision/v1.0/analyze?" + queryString;

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
    }
}
