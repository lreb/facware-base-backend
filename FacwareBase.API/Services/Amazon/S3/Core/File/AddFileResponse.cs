using System.Collections.Generic;

namespace FacwareBase.API.Services.Amazon.S3.Core.File
{
    public class AddFileResponse
    {
        public IList<string> PreSignedUrl { get; set; }
    }
}