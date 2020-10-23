using System.IO;
using System.Threading.Tasks;

namespace FacwareBase.API.Core.File.Management
{
	public interface IFileManagement
	{
		/// <summary>
		/// Read file from a temporal path
		/// </summary>
		/// <param name="temporalPath">temporal path</param>
		/// <param name="fileName">file name</param>
		/// <returns><see cref="MemoryStream"/>Memory stream</returns>
		Task<MemoryStream> ReadFileAsync(string temporalPath, string fileName);

		/// <summary>
		/// Get mime type 
		/// </summary>
		/// <param name="fileName">file name</param>
		/// <returns>string mime type</returns>
		string GetMimeType(string fileName);

		/// <summary>
		/// Remove file from a location
		/// </summary>
		/// <param name="path">file path</param>
		/// <param name="fileName">file name</param>
		void RemoveFile(string path, string fileName);

		/// <summary>
		/// Move file to different location
		/// </summary>
		/// <param name="originPath">original path</param>
		/// <param name="originFileName">origin file name</param>
		/// <param name="targetPath">target path</param>
		/// <param name="targetFileName">target file name</param>
		/// <returns>full target path</returns>
		string MoveFile(string originPath, string originFileName, string targetPath, string targetFileName);
	}
}
