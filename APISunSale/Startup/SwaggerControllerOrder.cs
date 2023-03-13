using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace APISunSale.Startup
{
    public class SwaggerControllerOrder : IDocumentFilter
    {
        public OpenApiDocument Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var orderedControllers = context.ApiDescriptions
                .Select(apiDesc => apiDesc.ActionDescriptor.DisplayName)
                .OrderBy(controllerName => controllerName?.Split('.').Last() != "Token")
                .ThenBy(controllerName => controllerName)
                .ToList();

            var paths = swaggerDoc.Paths
                .OrderBy(pair => orderedControllers.IndexOf(pair.Key.Split('/').First())).ToList();

            var list = new OpenApiPaths();
            foreach (var path in paths)
            {
                list.Add(path.Key, path.Value);
            }

            swaggerDoc.Paths = list;

            return swaggerDoc;
        }

        void IDocumentFilter.Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var orderedControllers = context.ApiDescriptions
                .Select(apiDesc => apiDesc.ActionDescriptor.DisplayName)
                .OrderBy(controllerName => controllerName?.Split('.').Last() != "Token")
                .ThenBy(controllerName => controllerName)
                .ToList();

            var paths = swaggerDoc.Paths
                .OrderBy(pair => orderedControllers.IndexOf(pair.Key.Split('/').First())).ToList();

            var list = new OpenApiPaths();
            foreach (var path in paths)
            {
                list.Add(path.Key, path.Value);
            }

            swaggerDoc.Paths = list;
        }
    }
}
