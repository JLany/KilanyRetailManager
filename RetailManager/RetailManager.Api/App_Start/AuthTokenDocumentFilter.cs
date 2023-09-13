using Swashbuckle.Swagger;
using System.Collections.Generic;
using System.Web.Http.Description;

namespace RetailManager.Api.App_Start
{
	public class AuthTokenDocumentFilter : IDocumentFilter
	{
		public void Apply(SwaggerDocument swaggerDoc
			, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
		{
			swaggerDoc.paths.Add("/api/Token", new PathItem
			{
				post = new Operation
				{
					tags = new List<string> { "Authentication" },
					consumes = new List<string> { "application/x-www-form-urlencoded" },
					parameters = new List<Parameter>
					{
						new Parameter
						{
							type = "string",
							name = "grant_type",
							required = true,
							@in = "formData",
							@default = "password"
						},
						new Parameter
						{
							type = "string",
							name = "username",
							required = false,
							@in = "formData"
						},
						new Parameter
						{
							type = "string",
							name = "password",
							required = false,
							@in = "formData"
						},
					}
				}
			});
		}
	}
}