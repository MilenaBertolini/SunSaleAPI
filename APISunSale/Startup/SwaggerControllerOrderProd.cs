using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace APISunSale.Startup
{
    public class SwaggerControllerOrderProd : IDocumentFilter
    {
        void IDocumentFilter.Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var paths = swaggerDoc.Paths.Where(p => p.Key.Contains("PublicQuestoes") || p.Key.Contains("Image") || p.Key.Contains("ForDevPublic")).ToList();

            var list = new OpenApiPaths();
            foreach (var path in paths)
            {
                list.Add(path.Key, path.Value);
            }

            swaggerDoc.Paths = list;
        }
    }
}
