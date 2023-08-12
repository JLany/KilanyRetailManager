using Swashbuckle.Swagger;
using System.Collections.Generic;
using System.Web.Http.Description;

namespace RetailManager.Api.App_Start
{
	public class AddAuthorizationHeaderParameterOperationFilter : IOperationFilter
	{
		public void Apply(Operation operation
			, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
		{
			if (operation.parameters is null)
			{
				operation.parameters = new List<Parameter>();
			}

			operation.parameters.Add(new Parameter
			{
				type = "string",
				name = "Authorization",
				@in = "header",
				description = "access token",
				required = false,
			});
		}
	}
}