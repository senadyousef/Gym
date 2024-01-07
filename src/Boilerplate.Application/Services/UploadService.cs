using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Boilerplate.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web; 
using System.Drawing; 
using System.Net;
using System.Net.Http;

namespace Boilerplate.Application.Services
{
    public class UploadService : IUploadService
    {
        public class UploadOptions
        {
            public string AWSAccessKeyId { get; set; }
            public string AWSSecretKey { get; set; }
            public string AWSBucketName { get; set; }
            public bool IsTestEnviroment { get; set; }

        }
        public UploadService(IOptions<UploadOptions> config)
        {
            _config = config.Value;
        }
         
        public async Task<string>  UploadImageAsync(string Image)
        {
            string message = null;
            string domain = "http://gym.useitsmart.com";
            string username = "78281734";
            string password = "Senad1979";
            string folderPath = "webimages";
            string imageUrl = Image; // Replace with your base64 image data

            using (var webClient = new WebClient())
            {
                //webClient.Credentials = new NetworkCredential(username, password);

                // Convert base64 string to byte array
                byte[] imageBytes = Convert.FromBase64String(imageUrl);

                // Upload the image to the server

                try
                {
                    string ftpServerIp = "160.153.155.203"; // Replace with the actual IP address
                    //string uploadUrl = $"ftp://{domain}/{folderPath}/image.png";
                    string uploadUrl = $"ftp://{ftpServerIp}/{folderPath}/{"image.png"}";
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uploadUrl);
                    request.Method = WebRequestMethods.Ftp.UploadFile;
                    request.Credentials = new NetworkCredential(username, password);

                    // Set passive mode
                    request.EnableSsl = true;

                    request.UsePassive = true;

                    using (Stream ftpStream = request.GetRequestStream())
                    {
                        ftpStream.Write(imageBytes, 0, imageBytes.Length);
                    }

                    //string uploadUrl = $"ftp://{domain}/{folderPath}/{"image.png"}";

                   
                    //webClient.UsePassive = true;

                    // Upload the file
                    webClient.UploadData(uploadUrl, "STOR", imageBytes);
                    Console.WriteLine("Image uploaded successfully.");
                    message = "Image uploaded successfully.";
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error uploading image: {ex.Message}");
                    message = $"Error uploading image: {ex.Message}";
                }
            }

            //// Create a HttpClient with basic authentication
            //using (var httpClient = new HttpClient())
            //{
            //    var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
            //    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);

            //    // Convert base64 string to byte array
            //    byte[] imageBytes = Convert.FromBase64String(imageUrl);

            //    // Upload the image to the server
            //    try
            //    {
            //        // Construct the URL for uploading
            //        //string uploadUrl = $"https://ftp.{domain}/{folderPath}/{"image.png"}";

            //        string uploadUrl = $"ftp://{domain}/{folderPath}/{"image.png"}";

            //        // Upload the file
            //        webClient.UploadData(uploadUrl, "STOR", imageBytes);
            //        Console.WriteLine("Image uploaded successfully.");
            //        message = "Image uploaded successfully.";
            //        // Create a MemoryStream from the byte array
            //        //using (var stream = new MemoryStream(imageBytes))
            //        //{
            //        //    // Create a stream content for the file
            //        //    using (var fileContent = new StreamContent(stream))
            //        //    {
            //        //        // Send a PUT request to the server
            //        //        using (var response = await httpClient.PutAsync(uploadUrl, fileContent))
            //        //        {
            //        //            response.EnsureSuccessStatusCode();
            //        //            Console.WriteLine("Image uploaded successfully.");
            //        //            message = "Image uploaded successfully.";
            //        //        }
            //        //    }
            //        //}
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine($"Error uploading image: {ex.Message}");
            //        message = $"Error uploading image: {ex.Message}";
            //    }
            //}
            return message;


        }

        public string UploadPhoto(string base64Data)
        {
            //string apiKey = "YourApiKey";
            //string apiSecret = "YourApiSecret";
            //string apiUrl = "https://api.godaddy.com/v1/your/endpoint";

            //using (HttpClient client = new HttpClient())
            //{
            //    // Set up headers with API key and secret
            //    client.DefaultRequestHeaders.Add("Authorization", $"sso-key {apiKey}:{apiSecret}");

            //    // Make a sample GET request (adjust for your specific API endpoint)
            //    HttpResponseMessage response = await client.GetAsync(apiUrl);

            //    if (response.IsSuccessStatusCode)
            //    {
            //        string result = await response.Content.ReadAsStringAsync();
            //        Console.WriteLine(result);
            //    }
            //    else
            //    {
            //        Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
            //    }
            //}
            return "";

            //string message = string.Empty;
            //string ftpServer = "http://gym.useitsmart.com/";
            //string ftpUsername = "78281734";
            //string ftpPassword = "Senad1979";
            //string targetFolder = "webimages/";
            //string base64Image = base64Data;

            //// Step 1: Convert Base64 string to byte array
            //byte[] imageBytes = Convert.FromBase64String(base64Image);

            //// Step 2: Save the byte array to a file
            //string fileName = "image.png"; // You can change the file name as needed
            //string filePath = Path.Combine(targetFolder, fileName);
            //File.WriteAllBytes(ftpServer + filePath, imageBytes);

            //// Step 3: Upload the file to GoDaddy server using FTP 
            //using (WebClient client = new WebClient())
            //{
            //    client.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
            //    client.UploadFile($"ftp://{ftpServer}/{targetFolder}/{fileName}", WebRequestMethods.Ftp.UploadFile, filePath);
            //}
            //return message;
        }

        //public bool SaveImage(string ImgStr, string ImgName)
        //{ 
        //   String path = HttpContext.Current.Server.MapPath(@"wwwroot\WebImages"); //Path

        //    //Check if directory exist
        //    if (!System.IO.Directory.Exists(path))
        //    {
        //        System.IO.Directory.CreateDirectory(path); //Create directory if it doesn't exist
        //    }

        //    string imageName = ImgName + ".jpg";

        //    //set the image path
        //    string imgPath = Path.Combine(path, imageName);

        //    byte[] imageBytes = Convert.FromBase64String(ImgStr);

        //    File.WriteAllBytes(imgPath, imageBytes);

        //    return true;
        //}

        public string UploadAsync(UploadRequest request)
        {
            if (request.Data == null) return string.Empty;
            var streamData = new MemoryStream(request.Data);
            if (streamData.Length > 0)
            {

                var folder = request.UploadType.ToDescriptionString();
                var folderName = NormalizePath(Path.Combine("Files", folder));
                var pathToSave = NormalizePath(Path.Combine(Directory.GetCurrentDirectory(), folderName));
                bool exists = System.IO.Directory.Exists(pathToSave);
                if (!exists)
                    System.IO.Directory.CreateDirectory(pathToSave);
                var fileName = request.FileName.Trim('"');
                var fullPath = NormalizePath(Path.Combine(pathToSave, fileName));
                var dbPath = NormalizePath(Path.Combine(folderName, fileName));
                if (File.Exists(dbPath))
                {
                    dbPath = NextAvailableFilename(dbPath);
                    fullPath = NextAvailableFilename(fullPath);
                }


                if (_config.IsTestEnviroment == true)
                {
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        streamData.CopyTo(stream);
                    }
                }
                else
                {

                    folder = request.UploadType.ToDescriptionString();
                    folderName = NormalizePath(Path.Combine("elasticbeanstalk-eu-west-1-827454755467/Files", folder));
                    fileName = request.FileName.Trim('"');
                    fullPath = NormalizePath(Path.Combine("https://elasticbeanstalk-eu-west-1-827454755467.s3.eu-west-1.amazonaws.com/Files", folder));
                    dbPath = NormalizePath(Path.Combine(fullPath, fileName));
                    using (var client = new AmazonS3Client("AKIA4BKBFSKFUIWAN2VM", "DLV1jFPtyHBxSo74k0sa24wlgKNUy5cW0pcUbjqg", RegionEndpoint.EUWest1))
                    { 
                        var uploadRequest = new TransferUtilityUploadRequest
                        {
                            InputStream = streamData,
                            Key = fileName, // filename
                            BucketName = folderName, // bucket name of S3
                            CannedACL = S3CannedACL.PublicReadWrite
                        };

                        var fileTransferUtility = new TransferUtility(client);
                        fileTransferUtility.UploadAsync(uploadRequest);

                    }
                }

                return dbPath;
            }
            else
            {
                return string.Empty;
            }
        }

        private static string numberPattern = " ({0})";
        private UploadOptions _config;




        public string NormalizePath(string path)
        {

            // If on Windows...
            //if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                if (false)

                    // Replace any / with \
                    return path?.Replace('/', '\\').Trim();
            // If on Linux/Mac
            else
                // Replace any \ with /
                return path?.Replace('\\', '/').Trim();
        }
        public static string NextAvailableFilename(string path)
        {
            // Short-cut if already available
            if (!File.Exists(path))
                return path;

            // If path has extension then insert the number pattern just before the extension and return next filename
            if (Path.HasExtension(path))
                return GetNextFilename(path.Insert(path.LastIndexOf(Path.GetExtension(path)), numberPattern));

            // Otherwise just append the pattern to the path and return next filename
            return GetNextFilename(path + numberPattern);
        }

        private static string GetNextFilename(string pattern)
        {
            string tmp = string.Format(pattern, 1);
            //if (tmp == pattern)
            //throw new ArgumentException("The pattern must include an index place-holder", "pattern");

            if (!File.Exists(tmp))
                return tmp; // short-circuit if no matches

            int min = 1, max = 2; // min is inclusive, max is exclusive/untested

            while (File.Exists(string.Format(pattern, max)))
            {
                min = max;
                max *= 2;
            }

            while (max != min + 1)
            {
                int pivot = (max + min) / 2;
                if (File.Exists(string.Format(pattern, pivot)))
                    min = pivot;
                else
                    max = pivot;
            }

            return string.Format(pattern, max);
        }
    }
}
