using System.Collections.Generic;
using System.Threading.Tasks;
using FacwareBase.API.Services.Amazon.S3.Core.Bucket;

namespace FacwareBase.API.Services.Amazon.S3.Core.Interfaces
{
    /// <summary>
    /// Manage buckets in s3
    /// </summary>
    public interface IBucketRepository
    {
        /// <summary>
        /// Validate if the bucket exist
        /// </summary>
        /// <param name="bucketName">Bucket name</param>
        /// <returns>true or false</returns>
        Task<bool> DoesS3BucketExist(string bucketName);

        /// <summary>
        /// Create a bucket
        /// </summary>
        /// <param name="bucketName">name of bucket</param>
        /// <returns>bucket response <see cref="CreateBucketResponse"/></returns>
        Task<CreateBucketResponse> CreateBucket(string bucketName);

        /// <summary>
        /// List all buckets
        /// </summary>
        /// <returns>List s3 objects <see cref="ListS3BucketsResponse"/></returns>
        Task<IEnumerable<ListS3BucketsResponse>> ListBuckets();

        /// <summary>
        /// Delete bucket
        /// </summary>
        /// <param name="bucketName">Bucket name</param>
        Task DeleteBucket(string bucketName);
    }
}