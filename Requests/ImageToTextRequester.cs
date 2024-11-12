using System.Net.Http;
using System.Net.Http.Json;
using System.Windows.Media.Imaging;
using EzCollege.Models;
using EzCollege.Helpers;

namespace EzCollege.Requests
{
    public class ImageToTextRequester
    {
        private static string? API_KEY;
        private const string API_URL = "https://api.api-ninjas.com/v1/imagetotext";

        public ImageToTextRequester()
        {
            API_KEY = Environment.GetEnvironmentVariable("API_NINJAS_KEY")!;
        }

        public static async Task<string> GetTextFromImage(BitmapSource image)
        {
            using HttpClient client = new();
            using MultipartFormDataContent formData = new();

            HttpContent fileStreamContent = Helper.ConvertImageToContent(image);
            fileStreamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
            fileStreamContent.Headers.Add("X-Api-Key", API_KEY);
            formData.Add(fileStreamContent, "image");

            var response = await client.PostAsync(API_URL, formData);

            Console.WriteLine(response.IsSuccessStatusCode);

            var jsonResponse = await response.Content.ReadFromJsonAsync<ITTResponseModel[]>();
            string text = string.Empty;
            jsonResponse!.ToList().ForEach(x => text += x.text + " ");

            return text;
        }
    }
}
