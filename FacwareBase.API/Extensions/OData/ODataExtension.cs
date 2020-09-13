using System;
using FacwareBase.API.Helpers.Domain.POCO;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNetCore.Builder;
using Microsoft.OData.Edm;

namespace FacwareBase.Api.Extensions.OData
{
	/// <summary>
	/// OData extension
	/// </summary>
	public static class ODataExtension
	{
		/// <summary>
		/// Generates the OData IEdm Models based on the DB Tables/Models
		/// </summary>
		/// <param name="serviceProvider"></param>
		/// <returns><see cref="IEdmModel"/></returns>
		public static IEdmModel GetODataModels(IServiceProvider serviceProvider)
		{
			ODataModelBuilder builder = new ODataConventionModelBuilder(serviceProvider).EnableLowerCamelCase();

			builder.EntitySet<Song>("Songs").EntityType.Filter();
			builder.EntitySet<Album>("Albums");

			return builder.GetEdmModel();
		}

    /// <summary>
		/// Generates the OData IEdm Models based on the DB Tables/Models extension method
		/// </summary>
		/// <param name="app"></param>
		/// <returns><see cref="IEdmModel"/></returns>
    public static IEdmModel GetODataModels(this IApplicationBuilder app)
		{
			ODataModelBuilder builder = new ODataConventionModelBuilder(app.ApplicationServices).EnableLowerCamelCase();

			builder.EntitySet<Song>("Songs");
			builder.EntitySet<Album>("Album");

      return builder.GetEdmModel();
	  }
  }
}