using System;

namespace FacwareBase.API.Services.Amazon.S3.Core.Bucket
{
    public class ListS3BucketsResponse
    {
        public string BucketName { get; set; }
        public DateTime CreationDate { get; set; }
    }
}