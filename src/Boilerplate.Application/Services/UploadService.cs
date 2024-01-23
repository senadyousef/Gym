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
        public async Task<string> UploadImageAsync(UploadPhotoRequest photoRequest)
        {
            string result = string.Empty;
            string wwwRootPath = string.Empty;
            string imageName = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.TimeOfDay.ToString();
            imageName = imageName.Replace(" ", "");
            imageName = imageName.Replace(":", "");
            imageName = imageName.Replace(".", "");
            string image = string.Empty;
            try
            {
                if (photoRequest.Data.StartsWith("data:image/png;base64,"))
                {
                    image = photoRequest.Data.Substring("data:image/png;base64,".Length);
                }
                else if (photoRequest.Data.StartsWith("data:image/jpeg;base64,"))
                {
                    image = photoRequest.Data.Substring("data:image/jpeg;base64,".Length);
                }
                else if (photoRequest.Data.StartsWith("data:image/jpg;base64,"))
                {
                    image = photoRequest.Data.Substring("data:image/jpg;base64,".Length);
                }
                else
                {
                    image = photoRequest.Data;
                }
                imageName = photoRequest.UploadType + "_" + imageName + "." + photoRequest.Extension;

                wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/WebImages/" + photoRequest.UploadType);
                result = "http://gym.useitsmart.com/webimages/" + photoRequest.UploadType + "/" + imageName; 

                byte[] imageBytes = Convert.FromBase64String(image);
                string imagePath = Path.Combine(wwwRootPath, imageName);
                if (!Directory.Exists(wwwRootPath))
                {
                    Directory.CreateDirectory(wwwRootPath);
                }
                System.IO.File.WriteAllBytes(imagePath, imageBytes);

            }
            catch (Exception ex)
            {
                result = "Error";
            }

            return result;
        }

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
