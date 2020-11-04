using System;

namespace FacwareBase.API.Services.Amazon.S3.Core.File
{
    public class GetJsonObjectResponse
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Last update
        /// </summary>
        public DateTime TimeSent { get; set; }
        /// <summary>
        /// Json data
        /// </summary>
        public string Data { get; set; }
    }
}