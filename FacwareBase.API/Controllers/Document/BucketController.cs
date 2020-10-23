using FacwareBase.API.Services.Amazon.S3.Core.Bucket;
using FacwareBase.API.Services.Amazon.S3.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FacwareBase.API.Controllers.Document
{
	[Route("api/bucket")]
	[ApiController]
	public class BucketController : ControllerBase
	{
		private readonly IBucketRepository _bucketRepository;

		public BucketController(IBucketRepository bucketRepository)
		{
			_bucketRepository = bucketRepository;
		}

		/// <summary>
		/// create a new s3 bucket
		/// </summary>
		/// <param name="bucketName"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("create/{bucketName}")]
		public async Task<ActionResult<CreateBucketResponse>> CreateS3Bucket([FromRoute] string bucketName)
		{
			var bucketExists = await _bucketRepository.DoesS3BucketExist(bucketName);

			if (bucketExists)
			{
				return BadRequest("S3 bucket already exists");
			}

			var result = await _bucketRepository.CreateBucket(bucketName);

			if (result == null)
			{
				return BadRequest();
			}

			return Ok(result);
		}

		/// <summary>
		/// list all s3 buckets
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Route("list")]
		public async Task<ActionResult<IEnumerable<ListS3BucketsResponse>>> ListS3Buckets()
		{
			var result = await _bucketRepository.ListBuckets();

			if (result == null)
			{
				return NotFound();
			}

			return Ok(result);
		}


		/// <summary>
		/// delete a s3 bucket
		/// </summary>
		/// <param name="bucketName"></param>
		/// <returns></returns>
		[HttpDelete]
		[Route("delete/{bucketName}")]
		public async Task<IActionResult> DeleteS3Bucket(string bucketName)
		{
			await _bucketRepository.DeleteBucket(bucketName);

			return Ok();
		}
	}
}
