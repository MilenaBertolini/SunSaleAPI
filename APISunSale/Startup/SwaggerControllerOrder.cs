﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace APISunSale.Startup
{
    public class SwaggerControllerOrder : IDocumentFilter
    {
        void IDocumentFilter.Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var paths = swaggerDoc.Paths
                .OrderBy(pair => !pair.Key.Contains("Token"))
                .ThenBy(pair => pair.Key)
                .ToList();



            var list = new OpenApiPaths();
            foreach (var path in paths)
            {
                list.Add(path.Key, path.Value);
            }

            swaggerDoc.Paths = list;
        }
    }
}
