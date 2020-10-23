using System;

namespace FacwareBase.API.Services.Amazon.S3.Core.File
{
    public class GetJsonObjectResponse
    {
        public Guid Id { get; set; }
        public DateTime TimeSent { get; set; }
        public string Data { get; set; }
    }
}