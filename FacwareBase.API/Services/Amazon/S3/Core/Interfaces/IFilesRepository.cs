using System.Collections.Generic;
using System.Threading.Tasks;
using FacwareBase.API.Services.Amazon.S3.Core.File;
using Microsoft.AspNetCore.Http;

namespace FacwareBase.API.Services.Amazon.S3.Core.Interfaces
{
    public interface IFilesRepository
    {
        Task<AddFileResponse> UploadFiles(string bucketName, IList<IFormFile> formFiles);
        Task<IEnumerable<ListFilesResponse>> ListFiles(string bucketName);
        Task DownloadFile(string bucketName, string fileName);
        Task<DeleteFileResponse> DeleteFile(string bucketName, string fileName);
        Task AddJsonObject(string bucketName, AddJsonObjectRequest request);

        Task<GetJsonObjectResponse> GetJsonObject(string bucketName, string fileName);
    }
}