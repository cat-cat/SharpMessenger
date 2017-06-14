
using System.IO;
using System.Threading.Tasks;

using ChatClient.Core.Common.Models.Base;

namespace ChatClient.Core.Common.Interfaces
{
	public interface IBaseUrl { string Get(); }

    public interface IFileHelper
    {
        string GetLocalFilePath(string filename);
        byte[] ReadAllBytes(string path);

        string FileName(string path);

        //Stream FileStream(string path);
        string CopyFile(string sourceFile, string destinationFilename, bool overwrite = true);

        Task<string> PhotoCache(string url, string fileName, ImageType imageType);

        string GetImageSrc(string fileName);
    }
}
