using FacwareBase.API.Services.Amazon.S3.Core.File;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FacwareBase.API.Services.Amazon.S3.Core.Interfaces
{
	public interface IFilesRepository
    {
        /// <summary>
        /// Upload files
        /// </summary>
        /// <param name="bucketName">bucket name</param>
        /// <param name="formFiles">files</param>
        /// <param name="key">s3 key</param>
        /// <returns>AddFileResponse<see cref="AddFileResponse"/></returns>
        Task<AddFileResponse> UploadFiles(string bucketName, IList<IFormFile> formFiles, string key = "");

        /// <summary>
        /// List all key/files in a s3 bucket
        /// </summary>
        /// <param name="bucketName">bucket name</param>
        /// <returns>ListFilesResponse<see cref="ListFilesResponse"/></returns>
        Task<IEnumerable<ListFilesResponse>> ListFiles(string bucketName);

        /// <summary>
        /// Download file from s3 to local temp folder
        /// </summary>
        /// <param name="bucketName">bucket name</param>
        /// <param name="fileName">file name</param>
        /// <param name="temporalPath">temporal path to download</param>
        /// <param name="key">s3 key: path</param>
        /// <returns>task just to download file</returns>
        Task DownloadFile(string bucketName, string fileName, string temporalPath, string key = "");

        /// <summary>
        /// Delete file from s3
        /// </summary>
        /// <param name="bucketName">bucket name</param>
        /// <param name="fileName">file name /key</param>
        /// <returns>DeleteFileResponse<see cref="DeleteFileResponse"/></returns>
        Task<DeleteFileResponse> DeleteFile(string bucketName, string fileName);
        
        /// <summary>
        /// Add json object
        /// </summary>
        /// <param name="bucketName">bucket name</param>
        /// <param name="request">object request</param>
        Task AddJsonObject(string bucketName, AddJsonObjectRequest request);

        /// <summary>
        /// Get json object
        /// </summary>
        /// <param name="bucketName">bucket name</param>
        /// <param name="fileName">file name</param>
        /// <returns>GetJsonObjectResponse<see cref="GetJsonObjectResponse"/></returns>
        Task<GetJsonObjectResponse> GetJsonObject(string bucketName, string fileName);
    }
}