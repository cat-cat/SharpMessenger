using System;
using Xamarin.Forms;
using System.Reflection;
using System.IO;

namespace ChatClient.Core.Common.Helpers
{
	public class FileReadHelper
	{
		public FileReadHelper()
		{
		}

		public string ReadEmbeddedFile(string filename) {
			var assembly = typeof(FileReadHelper).GetTypeInfo().Assembly;
			Stream stream = assembly.GetManifestResourceStream("ChatClient.Core.Common." + filename);
			string text = "";
			using (var reader = new System.IO.StreamReader (stream)) {
			    text = reader.ReadToEnd ();
			}

			return text;
		}
	}
}
