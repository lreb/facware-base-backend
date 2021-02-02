using Microsoft.AspNetCore.StaticFiles;
using System.IO;
using System.Threading.Tasks;

namespace FacwareBase.API.Core.File.Management
{
	public class FileManagement : IFileManagement
	{
		/// <summary>
		/// Read file from a temporal path
		/// </summary>
		/// <param name="path">temporal path</param>
		/// <param name="fileName">file name</param>
		/// <returns><see cref="MemoryStream"/>Memory stream</returns>
		public async Task<MemoryStream> ReadFileAsync(string path, string fileName)
		{
			string fullPath = Path.Combine(path, fileName);

			if (!System.IO.File.Exists(fullPath))
			{
				throw new FileNotFoundException();
			}

			var memory = new MemoryStream();
			await using (var stream = new FileStream(fullPath, FileMode.Open))
			{
				await stream.CopyToAsync(memory);
			}
			memory.Position = 0;

			return memory;
		}
		
		/// <summary>
		/// Get mime type 
		/// </summary>
		/// <param name="fileName">file name</param>
		/// <returns>string mime type</returns>
		public string GetMimeType(string fileName)
		{
			var provider = new FileExtensionContentTypeProvider();
			if (!provider.TryGetContentType(fileName, out var contentType))
			{
				contentType = "application/octet-stream";
			}
			return contentType;
		}

		/// <summary>
		/// Remove file from a location
		/// </summary>
		/// <param name="path">file path</param>
		/// <param name="fileName">file name</param>
		public void RemoveFile(string path, string fileName)
		{
			var fullPath = Path.Combine(path, fileName);

			if (System.IO.File.Exists(fullPath))
				System.IO.File.Delete(fullPath);
		}

		/// <summary>
		/// Move file to different location
		/// </summary>
		/// <param name="originPath">original path</param>
		/// <param name="originFileName">origin file name</param>
		/// <param name="targetPath">target path</param>
		/// <param name="targetFileName">target file name</param>
		/// <returns>full target path</returns>
		public string MoveFile(string originPath, string originFileName, string targetPath, string targetFileName)
		{
			var fullOriginPath = Path.Combine(originPath, originFileName);
			var fullTargetPath = Path.Combine(targetPath, targetFileName);

			if (!Directory.Exists(targetPath))
			{
				Directory.CreateDirectory(targetPath);
			}

			if (System.IO.File.Exists(fullOriginPath))
			{
				if (System.IO.File.Exists(fullTargetPath))
					System.IO.File.Delete(fullTargetPath);

				System.IO.File.Move(fullOriginPath, fullTargetPath);
			}
			return fullTargetPath;
		}
	}
}
