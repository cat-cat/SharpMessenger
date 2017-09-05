using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

using ChatClient.Core.Common.Interfaces;
using ChatClient.Core.Common.Models.Base;
using Windows.Storage;

using Xamarin.Forms;
using ChatClient.Core.DAL.Data.Base;
using ChatClient.UWP;

[assembly: Dependency(typeof(FileHelper))]
[assembly: Dependency(typeof(BaseUrl_UWP))]
namespace ChatClient.UWP
{
    public class BaseUrl_UWP : IBaseUrl
    {
        public string Get()
        {
            return ApplicationData.Current.LocalFolder.Path;
        }
    }


    public class FileHelper : IFileHelper
    {
        public async Task<string> GetLocalFilePath(string filename)
        {
            string docFolder = ApplicationData.Current.LocalFolder.Path;
            string libFolder = Path.Combine(docFolder, "Library");

            if (!Directory.Exists(libFolder))
            {
                await Task.Run(() => { Directory.CreateDirectory(libFolder); });
            }

            return Path.Combine(libFolder, filename);
        }
        public byte[] ReadAllBytes(string path)
        {
            return File.ReadAllBytes(path);
        }
        public string FileName(string path)
        {
            return Path.GetFileName(path);
        }
        //public Stream FileStream(string path)
        //{
        //    StreamReader lStreamReader = new StreamReader(path);
        //    MemoryStream lMemoryStream = new MemoryStream();
        //    lMemoryStream.Position = 0;
        //    lStreamReader.BaseStream.CopyTo(lMemoryStream);
        //    return lMemoryStream;
        //}

        public string CopyFile(string sourceFile, string destinationFilename, bool overwrite = true)
        {
            if (!File.Exists(sourceFile)) { return string.Empty; }
            string fullFileLocation = Path.Combine(ApplicationData.Current.LocalFolder.Path, destinationFilename);
            File.Copy(sourceFile, fullFileLocation, overwrite);
            return fullFileLocation;
        }

        public async Task<string> PhotoCache(string url, string fileName, ImageType imageType)
        {
            try
            {
                if (fileName == "winning_sum.png")
                {
                    return "winning_sum.png";
                }
                string lImageFolder = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Library", imageType.ToString());
                if (!Directory.Exists(lImageFolder))
                {
                    Directory.CreateDirectory(lImageFolder);
                }
                string lFilePath = Path.Combine(lImageFolder, fileName);
                if (File.Exists(lFilePath))
                    return lFilePath;
                SaveImage(lFilePath, url + fileName);
                return lFilePath;
            }
            catch
            {

                switch (imageType)
                {
                    case ImageType.Groups:
                        return "winning_sum.png";
                    case ImageType.Users:
                        return "profile_avatar.png";
                }
            }
            return "";
        }

        public string GetImageSrc(string fileName)
        {
            return fileName;
        }

        //TODO:
        private async void SaveImage(string path, string url)
        {
            HttpClient httpClient = new HttpClient();
            Uri lUri = new Uri(url);

            try
            {
                //Send the GET request
                HttpResponseMessage httpResponse = await httpClient.GetAsync(lUri);
                httpResponse.EnsureSuccessStatusCode();
                byte[] lResponseBytes = await httpResponse.Content.ReadAsByteArrayAsync();
                File.WriteAllBytes(path, lResponseBytes);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(String.Format("StackTrace:{0}\n Error:{1}", ex.StackTrace, ex.Message));
            }

        }
    }
}
