using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

using ChatClient.Core.Common.Interfaces;
using ChatClient.Core.Common.Models.Base;
using ChatClient.iOS;

using Xamarin.Forms;
using ChatClient.Core.DAL.Data.Base;

using Foundation;

using Plugin.Media.Abstractions;

using UIKit;

[assembly: Dependency(typeof(FileHelper))]
[assembly: Dependency (typeof (BaseUrl_iOS))]
namespace ChatClient.iOS
{
	public class BaseUrl_iOS : IBaseUrl {
		public string Get() {
		  return NSBundle.MainBundle.BundlePath;
		}
	}


    public class FileHelper : IFileHelper
    {
        public string GetLocalFilePath(string filename)
        {
            string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libFolder = Path.Combine(docFolder, "..", "Library", "Databases");

            if (!Directory.Exists(libFolder))
            {
                Directory.CreateDirectory(libFolder);
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
            string fullFileLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), destinationFilename);
            File.Copy(sourceFile, fullFileLocation, overwrite);
            return fullFileLocation;
        }

        public async Task<string> PhotoCache(string url, string fileName, ImageType imageType)
        {
            try
            {
                if (fileName == "winning_sum.png") {
                    return "winning_sum.png";
                }
                string lBaseFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                string lImageFolder = Path.Combine(lBaseFolder, "..", "Library", imageType.ToString());
                if (!Directory.Exists(lImageFolder))
                {
                    Directory.CreateDirectory(lImageFolder);
                }
                string lFilePath = Path.Combine(lImageFolder, fileName);
                if (File.Exists(lFilePath) )
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

        public string GetImageSrc(string fileName) {
            return fileName;
        }

        private void SaveImage(string path, string url)
        {
            WebClient lWebClient = new WebClient();
            Uri lUri = new Uri(url);
           
            lWebClient.DownloadDataCompleted += (s, e) =>
            {
                try
                {
                    byte[] lResponseBytes = e.Result; 
                    if (lResponseBytes.Length > 0)
                    {
                        File.WriteAllBytes(path, lResponseBytes);
                    }
                }
                catch (Exception lException)
                {
                }
            };
            try {
                lWebClient.DownloadDataAsync(lUri);

            }
            catch (Exception lException) {
                
            }
           
           


        }
    }
}
