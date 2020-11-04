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

    /// <summary>
    /// file data
    /// </summary>
    public class AddSingleFileResponse
    {
	    /// <summary>
	    /// url pre signed
	    /// </summary>
	    public string PreSignedUrl { get; set; }
	    /// <summary>
	    /// s3 key
	    /// </summary>
	    public string Key { get; set; }
    }
}