using System;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        await UploadPhoto("http://gym.useitsmart.com/", "webimages/");
    }

    static async Task UploadPhoto(string apiUrl, string imagePath)
    {
        using (var httpClient = new HttpClient())
        using (var content = new MultipartFormDataContent())
        using (var fileStream = System.IO.File.OpenRead(imagePath))
        {
            // Add the photo file to the request
            content.Add(new StreamContent(fileStream), "file", "photo.jpg");

            // You can add additional data as needed, such as authentication tokens or metadata
            // content.Add(new StringContent("your token"), "token");

            // Send the request
            var response = await httpClient.PostAsync(apiUrl, content);

            // Check the response
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Photo uploaded successfully!");
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
            }
        }
    }
}