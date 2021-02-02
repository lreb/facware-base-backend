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
	        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            _sessionAwsCredentialsOptions = sessionAwsCredentialsOptions.Value;
            if (env.Contains("Local"))
            {
	            // TODO: we must improve this client initialization, this code commented is just to test in the local environment. create a temporal session to test locally, use SSO temporal keys, update these keys in app settings
	            SessionAWSCredentials tempCredentials = new SessionAWSCredentials(_sessionAwsCredentialsOptions.AwsAccessKeyId,
		            _sessionAwsCredentialsOptions.AwsSecretAccessKey,
		            _sessionAwsCredentialsOptions.Token);
	            _s3Client = new AmazonS3Client(tempCredentials, RegionEndpoint.APSoutheast1);
            }
            else
            {
	            // TODO: example using IAM keys
	            // IAM
	            //var credentials = new BasicAWSCredentials(_sessionAwsCredentialsOptions.AwsAccessKeyId, _sessionAwsCredentialsOptions.AwsSecretAccessKey);
	            //_s3Client = new AmazonS3Client(RegionEndpoint.APSoutheast1);

	            // TODO: using IAM role, deployed to development, staging or production
	            _s3Client = s3Client;
            }
        }

        private static async Task<SessionAWSCredentials> GetTemporaryCredentialsAsync()
        {
			using var stsClient = new AmazonSecurityTokenServiceClient();
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
        /// Upload files
        /// </summary>
        /// <param name="bucketName">bucket name</param>
        /// <param name="formFiles">files</param>
        /// <param name="key">s3 key</param>
        /// <returns>AddFileResponse<see cref="AddFileResponse"/></returns>
        public async Task<AddFileResponse> UploadFiles(string bucketName, string key, params IFormFile[] formFiles)
        {
            var response = new List<string>();

            foreach (var file in formFiles)
            {
                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = file.OpenReadStream(),
                    Key = key,
                    BucketName = bucketName,
                    CannedACL = S3CannedACL.PublicRead,
                    StorageClass = S3StorageClass.Standard,
                    ContentType = file.ContentType
                };

                using (var fileTransferUtility = new TransferUtility(_s3Client))
                {
                    await fileTransferUtility.UploadAsync(uploadRequest);
                }

                var expiryUrlRequest = new GetPreSignedUrlRequest
                {
                    BucketName = bucketName,
                    Key = key,
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
        /// Copy a file or key to another location
        /// </summary>
        /// <param name="sourceBucket">source bucket name</param>
        /// <param name="sourceKey">source key</param>
        /// <param name="targetBucket">target bucket name</param>
        /// <param name="targetKey">traget key</param>
        /// <returns>target key</returns>
        public async Task<string> CopyKey(string sourceBucket, string sourceKey, string targetBucket, string targetKey)
        {
	        var copy = await _s3Client.CopyObjectAsync(sourceBucket, sourceKey, targetBucket, targetKey);
	        return targetKey;
        }

        /// <summary>
        /// Delete file from s3
        /// </summary>
        /// <param name="bucketName">bucket name</param>
        /// <param name="key">s3 key /key</param>
        /// <returns>DeleteFileResponse<see cref="DeleteFileResponse"/></returns>
        public async Task<DeleteFileResponse> DeleteFile(string bucketName, string key)
        {
	        var multiObjectDeleteRequest = new DeleteObjectsRequest
	        {
		        BucketName = bucketName
	        };

	        multiObjectDeleteRequest.AddKey(key);

	        var response = await _s3Client.DeleteObjectsAsync(multiObjectDeleteRequest);

	        return new DeleteFileResponse
	        {
		        NumberOfDeletedObjects = response.DeletedObjects.Count
	        };
        }

        /// <summary>
        /// Download file from s3 to local temp folder
        /// </summary>
        /// <param name="bucketName">bucket name</param>
        /// <param name="key">key = path + file name</param>
        /// <param name="temporalPath">temporal path to download + file name</param>
        /// <returns>task just to download file</returns>
        public async Task DownloadFile(string bucketName, string key, string temporalPath)
        {
			GetObjectRequest request = new GetObjectRequest
			{
				BucketName = bucketName,
				Key = key
			};

			var downloadRequest = new TransferUtilityDownloadRequest
            {
                BucketName = bucketName,
                Key = key,
                FilePath = temporalPath,
            };

            using var transferUtility = new TransferUtility(_s3Client);
            await transferUtility.DownloadAsync(downloadRequest);
        }

        /// <summary>
        /// Get file from s3 bucket
        /// </summary>
        /// <param name="bucketName">bucket name</param>
        /// <param name="key">s3 key -  path + file name</param>
        /// <returns><see cref="MemoryStream"/>Memory file object</returns>
        public async Task<MemoryStream> DownloadMemoryStreamAsync(string bucketName, string key)
        {
            var memory = new MemoryStream();
            try
            {
                var streamRequest = new TransferUtilityOpenStreamRequest
                {
                    BucketName = bucketName,
                    Key = key
                };

                var request = new GetObjectRequest()
                {
                    BucketName = bucketName,
                    Key = key
                };

                using (var transferUtility = new TransferUtility(_s3Client))
                {
                    var objectResponse = await transferUtility.S3Client.GetObjectAsync(request);

                    var stream = objectResponse.ResponseStream;

                    await stream.CopyToAsync(memory);
                }

                memory.Position = 0;

                return memory;
            }
            catch (AmazonS3Exception e)
            {
                Console.Write("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.Write("Download fail", e.Message);
            }

            return memory;
        }

        /// <summary>
        /// Get json object
        /// </summary>
        /// <param name="bucketName">bucket name</param>
        /// <param name="key">path + file name</param>
        /// <returns>GetJsonObjectResponse<see cref="GetJsonObjectResponse"/></returns>
        public async Task<GetJsonObjectResponse> GetJsonObject(string bucketName, string key)
        {
            var request = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = key
            };

            var response = await _s3Client.GetObjectAsync(request);

			using var reader = new StreamReader(response.ResponseStream);
			var contents = await reader.ReadToEndAsync();
			return JsonConvert.DeserializeObject<GetJsonObjectResponse>(contents);
		}

        /// <summary>
        /// Generate presigned url
        /// </summary>
        /// <param name="bucketName">bucket name</param>
        /// <param name="key">file key</param>
        /// <param name="expirationTime">time to expire url</param>
        /// <returns></returns>
        public AddSingleFileResponse GetPreSignedUrl(string bucketName, string key, DateTime expirationTime)
        {
	        var expiryUrlRequest = new GetPreSignedUrlRequest
	        {
		        BucketName = bucketName,
		        Key = key,
		        Expires = expirationTime
	        };

	        var url = _s3Client.GetPreSignedURL(expiryUrlRequest);

	        var response = new AddSingleFileResponse()
	        {
		        PreSignedUrl = url, // temporal download url
		        Key = key // Internal reference s3 key
	        };

	        return response;
        }
    }
}