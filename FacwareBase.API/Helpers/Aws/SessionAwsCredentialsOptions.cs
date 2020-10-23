namespace FacwareBase.API.Helpers.Aws
{
	/// <summary>
	/// AWS access parameters to create a new session
	/// </summary>
	public class SessionAwsCredentialsOptions
	{
		/// <summary>
		/// public key access
		/// </summary>
		public string AwsAccessKeyId { get; set; }
		/// <summary>
		/// private key access
		/// </summary>
		public string AwsSecretAccessKey { get; set; }
		/// <summary>
		/// Tempo token
		/// </summary>
		public string Token { get; set; }
	}
}
