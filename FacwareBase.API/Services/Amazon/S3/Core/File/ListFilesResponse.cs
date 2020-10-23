using System;
using Amazon.S3;

namespace FacwareBase.API.Services.Amazon.S3.Core.File
{
    /// <summary>
    /// S3 response object
    /// </summary>
    public class ListFilesResponse
    {
        /// <summary>
        /// S3 bucket name
        /// </summary>
        public string BucketName { get; set; }
        /// <summary>
        /// S3 key: e.g folder1/subfolder2/file.name
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// File owner
        /// </summary>
        public string Owner { get; set; }
        /// <summary>
        /// File size
        /// </summary>
        public long Size { get; set; }
        /// <summary>
        /// Last updated
        /// </summary>
        public DateTime LastModified { get; set; }
        /// <summary>
        /// Storage level class
        /// </summary>
        public S3StorageClass StorageClass { get; set; }
    }
}