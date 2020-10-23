using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using FacwareBase.API.Helpers.Aws;
using FacwareBase.API.Services.Amazon.S3.Core.Bucket;
using FacwareBase.API.Services.Amazon.S3.Core.Interfaces;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacwareBase.API.Services.Amazon.S3.Infraestructure.Repositories
{
	public class BucketRepository : IBucketRepository
    {
	    private readonly SessionAwsCredentialsOptions _sessionAwsCredentialsOptions;
	    private readonly IAmazonS3 _s3Client;
        
        public BucketRepository(IAmazonS3 s3Client,
			IOptions<SessionAwsCredentialsOptions> sessionAwsCredentialsOptions)
        {
	        _sessionAwsCredentialsOptions = sessionAwsCredentialsOptions.Value;
	        // TODO: we must improve this client initialization, this code commented is just to test in the local environment
            // create a temporal session to test locally, use SSO temporal keys, update these keys in app settings
            //   SessionAWSCredentials tempCredentials = new SessionAWSCredentials(_sessionAwsCredentialsOptions.AwsAccessKeyId,
            // _sessionAwsCredentialsOptions.AwsSecretAccessKey,
            // _sessionAwsCredentialsOptions.Token);
            //_s3Client = new AmazonS3Client(tempCredentials, RegionEndpoint.APSoutheast1);

            _s3Client = s3Client;
        }

        /// <summary>
        /// Validate if the bucket exist
        /// </summary>
        /// <param name="bucketName">Bucket name</param>
        /// <returns>true or false</returns>
        public async Task<bool> DoesS3BucketExist(string bucketName)
        {
            return await _s3Client.DoesS3BucketExistAsync(bucketName);
        }

        /// <summary>
        /// Create a bucket
        /// </summary>
        /// <param name="bucketName">name of bucket</param>
        /// <returns>bucket response <see cref="CreateBucketResponse"/></returns>
        public async Task<CreateBucketResponse> CreateBucket(string bucketName)
        {
            var putBucketRequest = new PutBucketRequest
            {
                BucketName = bucketName,
                UseClientRegion = true
            };

            var response = await _s3Client.PutBucketAsync(putBucketRequest);

            return new CreateBucketResponse
            {
                BucketName = bucketName,
                RequestId = response.ResponseMetadata.RequestId
            };
        }

        /// <summary>
        /// List all buckets
        /// </summary>
        /// <returns>List s3 objects <see cref="ListS3BucketsResponse"/></returns>
        public async Task<IEnumerable<ListS3BucketsResponse>> ListBuckets()
        {
            var response = await _s3Client.ListBucketsAsync();

            return response.Buckets.Select(b => new ListS3BucketsResponse
            {
                BucketName = b.BucketName,
                CreationDate = b.CreationDate
            });
        }

        /// <summary>
        /// Delete bucket
        /// </summary>
        /// <param name="bucketName">Bucket name</param>
        public async Task DeleteBucket(string bucketName)
        {
            await _s3Client.DeleteBucketAsync(bucketName);
        }
    }
}