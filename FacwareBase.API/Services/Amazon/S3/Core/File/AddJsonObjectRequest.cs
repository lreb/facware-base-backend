using System;
using Newtonsoft.Json;

namespace FacwareBase.API.Services.Amazon.S3.Core.File
{
    /// <summary>
    /// Json object
    /// </summary>
    public class AddJsonObjectRequest
    {
        /// <summary>
        /// object id
        /// </summary>
        [JsonProperty("id")]
        public Guid Id { get; set; }
        /// <summary>
        /// object created date time
        /// </summary>
        [JsonProperty("timesent")]
        public DateTime TimeSent { get; set; }
        /// <summary>
        /// Json object content
        /// </summary>
        [JsonProperty("data")]
        public string Data { get; set; }
    }
}