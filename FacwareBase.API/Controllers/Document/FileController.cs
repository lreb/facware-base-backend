using FacwareBase.API.Core.File.Management;
using FacwareBase.API.Helpers.FileManagement;
using FacwareBase.API.Services.Amazon.S3.Core.File;
using FacwareBase.API.Services.Amazon.S3.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FacwareBase.API.Controllers.Document
{
	[Route("api/file")]
    [ApiController]    
    public class FileController : ControllerBase
    {
        private readonly IFilesRepository _filesRepository;
        private readonly IFileManagement _fileManagement;
        private readonly FileStorageOptions _fileStorageOptions;

        public FileController(IFilesRepository filesRepository,
	        IFileManagement fileManagement,
	        IOptions<FileStorageOptions> fileStorageOptions)
        {
	        _filesRepository = filesRepository;
	        _fileManagement = fileManagement;
	        _fileStorageOptions = fileStorageOptions.Value;
        }

        /// <summary>
        /// Add new file
        /// </summary>
        /// <param name="bucketName">bucket name</param>
        /// <param name="key">s3 key</param>
        /// <param name="formFiles">file</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{bucketName}/add/{key}")]
        public async Task<ActionResult<AddFileResponse>> AddFiles(string bucketName, string key, IList<IFormFile> formFiles)
        {
            if (formFiles == null)
            {
                return BadRequest("The request doesn't contain any files to be uploaded.");
            }

            var response = await _filesRepository.UploadFiles(bucketName, key, formFiles[0]);

            if (response == null)
            {
                return BadRequest();
            }

            return Ok(response);
        }

        /// <summary>
        /// List keys in a s3 bucket
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{bucketName}/list")]
        public async Task<ActionResult<IEnumerable<ListFilesResponse>>> ListFiles(string bucketName)
        {
            var response = await _filesRepository.ListFiles(bucketName);

            return Ok(response);
        }

        /// <summary>
        /// Download file from s3 bucket
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{bucketName}/download/{fileName}")]
        public async Task<IActionResult> DownloadFile(string bucketName, string fileName)
        {
	        //var temporalPath = _fileStorageOptions.TemporalStorage;

         //   await _filesRepository.DownloadFile(bucketName, fileName, temporalPath);

         //   var memoryFile = _fileManagement.ReadFileAsync(temporalPath, fileName).Result;

         //   _fileManagement.RemoveFile(temporalPath, fileName);

         var key = "test123/";
			var memoryFile = await _filesRepository.DownloadMemoryStreamAsync(bucketName, key + fileName);

            var mimeType = _fileManagement.GetMimeType(fileName);

	        return File(memoryFile, mimeType, fileName);

            //return Ok();
        }
        
        /// <summary>
        /// Delete file from s3 bucket
        /// </summary>
        /// <param name="bucketName">Bucket name</param>
        /// <param name="fileName">file name</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{bucketName}/delete/{fileName}")]
        public async Task<ActionResult<DeleteFileResponse>> DeleteFile(string bucketName, string fileName)
        {
            var response = await _filesRepository.DeleteFile(bucketName, fileName);

            return Ok(response);
        }

        /// <summary>
        /// Add json object to s3 bucket
        /// </summary>
        /// <param name="bucketName">bucket</param>
        /// <param name="request">json object</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{bucketName}/addjsonobject")]
        public async Task<IActionResult> AddJsonObject(string bucketName, AddJsonObjectRequest request)
        {
            await _filesRepository.AddJsonObject(bucketName, request);

            return Ok();
        }

        /// <summary>
        /// Get json object
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{bucketName}/getjsonobject")]
        public async Task<ActionResult<GetJsonObjectResponse>> GetJsonObject(string bucketName, string fileName)
        {
            var response = await _filesRepository.GetJsonObject(bucketName, fileName);

            return Ok(response);
        }
    }
}