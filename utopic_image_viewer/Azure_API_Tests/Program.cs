using System.IO;
using System;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;
using System.Web;

namespace Azure_API_Tests
{
    class Program
    {
        static void Main(string[] args)
        {
                Console.Write("Enter the location of your picture:");
                string imageFilePath = Console.ReadLine();

                //Face.detect.MakeFaceRequestUri(imageFilePath);
                Cognitive.CV.MakeCVRequestUri(imageFilePath);

                Console.WriteLine("\nWait for the result below, then hit ENTER to exit...\n");
                Console.ReadLine();
        }


    }
}
