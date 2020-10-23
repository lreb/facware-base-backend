using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.SecurityToken;
using Amazon.SecurityToken.Model;
using FacwareBase.API.Helpers.Aws;
using FacwareBase.API.Services.Amazon.S3.Core.File;
using FacwareBase.API.Services.Amazon.S3.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FacwareBase.API.Services.Amazon.S3.Infraestructure.Repositories
{
	public class FilesRepository : IFilesRepository
    {
	    private readonly SessionAwsCredentialsOptions _sessionAwsCredentialsOptions;
        private readonly IAmazonS3 _s3Client;

        public FilesRepository(IAmazonS3 s3Client,
	        IOptions<SessionAwsCredentialsOptions> sessionAwsCredentialsOptions)
        {
	        _sessionAwsCredentialsOptions = sessionAwsCredentialsOptions.Value;
            // TODO: we must improve this client initialization, this code commented is just to test in the local environment
            // create a temporal session to test locally, use SSO temporal keys, update these keys in app settings
            //SessionAWSCredentials tempCredentials = new SessionAWSCredentials(_sessionAwsCredentialsOptions.AwsAccessKeyId,
            // _sessionAwsCredentialsOptions.AwsSecretAccessKey,
            // _sessionAwsCredentialsOptions.Token);
            //_s3Client = new AmazonS3Client(tempCredentials, RegionEndpoint.APSoutheast1);

            // IAM
            //var credentials = new BasicAWSCredentials(_sessionAwsCredentialsOptions.AwsAccessKeyId, _sessionAwsCredentialsOptions.AwsSecretAccessKey);
            //_s3Client = new AmazonS3Client(credentials, RegionEndpoint.USEast1);

            // deployed to development, staging or production
            _s3Client = s3Client;
        }

        private static async Task<SessionAWSCredentials> GetTemporaryCredentialsAsync()
        {
	        using (var stsClient = new AmazonSecurityTokenServiceClient())
	        {
		        var getSessionTokenRequest = new GetSessionTokenRequest
		        {
			        DurationSeconds = 7200 // seconds
		        };

		        GetSessionTokenResponse sessionTokenResponse =
			        await stsClient.GetSessionTokenAsync(getSessionTokenRequest);

		        Credentials credentials = sessionTokenResponse.Credentials;

		        var sessionCredentials =
			        new SessionAWSCredentials(credentials.AccessKeyId,
				        credentials.SecretAccessKey,
				        credentials.SessionToken);
		        return sessionCredentials;
	        }
        }

        /// <summary>
        /// Upload files
        /// </summary>
        /// <param name="bucketName">bucket name</param>
        /// <param name="formFiles">files</param>
        /// <param name="key">s3 key</param>
        /// <returns>AddFileResponse<see cref="AddFileResponse"/></returns>
        public async Task<AddFileResponse> UploadFiles(string bucketName, IList<IFormFile> formFiles, string key)
        {
            var response = new List<string>();

            var fullKey = key.Equals(string.Empty) ? key : $"{key}/";

            foreach (var file in formFiles)
            {
                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = file.OpenReadStream(),
                    Key = fullKey + file.FileName,
                    BucketName = bucketName,
                    CannedACL = S3CannedACL.NoACL
                };

                using (var fileTransferUtility = new TransferUtility(_s3Client))
                {
                    await fileTransferUtility.UploadAsync(uploadRequest);
                }

                var expiryUrlRequest = new GetPreSignedUrlRequest
                {
                    BucketName = bucketName,
                    Key = fullKey + file.FileName,
                    Expires = DateTime.Now.AddDays(1)
                };

                var url = _s3Client.GetPreSignedURL(expiryUrlRequest);

                response.Add(url);
            }

            return new AddFileResponse
            {
                PreSignedUrl = response
            };
        }

        /// <summary>
        /// List all key/files in a s3 bucket
        /// </summary>
        /// <param name="bucketName">bucket name</param>
        /// <returns>ListFilesResponse<see cref="ListFilesResponse"/></returns>
        public async Task<IEnumerable<ListFilesResponse>> ListFiles(string bucketName)
        {
            var responses = await _s3Client.ListObjectsAsync(bucketName);

            return responses.S3Objects.Select(b => new ListFilesResponse
            {
                BucketName = b.BucketName,
                Key = b.Key,
                Owner = b.Owner.DisplayName,
                Size = b.Size, 
                LastModified = b.LastModified,
                StorageClass = b.StorageClass
            });
        }

        /// <summary>
        /// Download file from s3 to local temp folder
        /// </summary>
        /// <param name="bucketName">bucket name</param>
        /// <param name="fileName">file name</param>
        /// <param name="temporalPath">temporal path to download</param>
        /// <param name="key">s3 key: path</param>
        /// <returns>task just to download file</returns>
        public async Task DownloadFile(string bucketName, string fileName, string temporalPath, string key = "")
        {
	        var fullKey = key.Equals(string.Empty) ? $"{fileName}" : $"{key}/{fileName}";

            GetObjectRequest request = new GetObjectRequest();
            request.BucketName = bucketName;
            request.Key = fullKey;
            // GetObjectResponse response = await _s3Client.GetObjectAsync(request);

            var downloadRequest = new TransferUtilityDownloadRequest
            {
                BucketName = bucketName,
                Key = fileName,
                FilePath = $"{temporalPath}{fileName}",
            };

			using var transferUtility = new TransferUtility(_s3Client);
			await transferUtility.DownloadAsync(downloadRequest);
		}

        /// <summary>
        /// Delete file from s3
        /// </summary>
        /// <param name="bucketName">bucket name</param>
        /// <param name="fileName">file name /key</param>
        /// <returns>DeleteFileResponse<see cref="DeleteFileResponse"/></returns>
        public async Task<DeleteFileResponse> DeleteFile(string bucketName, string fileName)
        {
            var multiObjectDeleteRequest = new DeleteObjectsRequest
            {
                BucketName = bucketName
            };

            multiObjectDeleteRequest.AddKey(fileName);

            var response = await _s3Client.DeleteObjectsAsync(multiObjectDeleteRequest);

            return new DeleteFileResponse
            {
                NumberOfDeletedObjects = response.DeletedObjects.Count
            };
        }

        /// <summary>
        /// Add json object
        /// </summary>
        /// <param name="bucketName">bucket name</param>
        /// <param name="request">object request</param>
        public async Task AddJsonObject(string bucketName, AddJsonObjectRequest request)
        {
            var createdOnUtc = DateTime.UtcNow;

            var s3Key = $"{createdOnUtc:yyyy}/{createdOnUtc:MM}/{createdOnUtc:dd}/{request.Id}";

            var putObjectRequest = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = s3Key,
                ContentBody = JsonConvert.SerializeObject(request)
            };

            await _s3Client.PutObjectAsync(putObjectRequest);
        }

        /// <summary>
        /// Get json object
        /// </summary>
        /// <param name="bucketName">bucket name</param>
        /// <param name="fileName">file name</param>
        /// <returns>GetJsonObjectResponse<see cref="GetJsonObjectResponse"/></returns>
        public async Task<GetJsonObjectResponse> GetJsonObject(string bucketName, string fileName)
        {
            var request = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = fileName
            };

            var response = await _s3Client.GetObjectAsync(request);

            using (var reader = new StreamReader(response.ResponseStream))
            {
                var contents = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<GetJsonObjectResponse>(contents);
            }
        }
    }
}