using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

using ChatClient.Core.Common.Interfaces;
using ChatClient.Core.Common.Models.Base;
using ChatClient.Core.DAL.Data.Base;
using ChatClient.Droid;

using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper))]
[assembly: Dependency (typeof(BaseUrl_Android))]
namespace ChatClient.Droid
{
	  public class BaseUrl_Android : IBaseUrl {
	    public string Get() {
	      return "file:///android_asset/";
	    }
	  }

    public class FileHelper : IFileHelper
    {
		public async Task<string> GetLocalFilePath(string filename)
		{
			string path = await Task.Run(() => { return Environment.GetFolderPath(Environment.SpecialFolder.Personal); });
            return Path.Combine(path, filename);
        }

        public byte[] ReadAllBytes(string path) {
			return File.ReadAllBytes(path);
        }

        public string FileName(string path) {
            string lFileName = Path.GetFileName(path);
            return lFileName ;
        }

        //public Stream FileStream(string path) {
        //    StreamReader lStreamReader = new StreamReader(path);
        //    MemoryStream lMemoryStream = new MemoryStream();
        //    lMemoryStream.Position = 0;
        //    lStreamReader.BaseStream.CopyTo(lMemoryStream);
        //    return lMemoryStream;
        //}

        public string CopyFile(string sourceFile, string destinationFilename, bool overwrite = true) {

            if (!File.Exists(sourceFile)) { return string.Empty; }
			string fullFileLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), destinationFilename);
			FileStream fs = File.Create(fullFileLocation);
			fs.Close();
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
				string lBaseFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                string lImageFolder = Path.Combine(lBaseFolder, imageType.ToString());
                if (!Directory.Exists(lImageFolder))
                {
                    Directory.CreateDirectory(lImageFolder);
                }
                string lFilePath = Path.Combine(lImageFolder, fileName);
                if (File.Exists(lFilePath) )
                    //if( File.ReadAllBytes(lFilePath).Length > 0)
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
            return Path.Combine("file:///android_asset",fileName);
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
                       
                       (s as WebClient).Dispose();
                    }
             
                }
                catch (Exception lException)
                {
                }
            };
            lWebClient.DownloadDataAsync(lUri);
        }
    }
}