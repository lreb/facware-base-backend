using System.Collections.Generic;

namespace FacwareBase.API.Services.Amazon.S3.Core.File
{
    /// <summary>
    /// New file response
    /// </summary>
    public class AddFileResponse
    {
        /// <summary>
        /// Uri
        /// </summary>
        public IList<string> PreSignedUrl { get; set; }
    }
}