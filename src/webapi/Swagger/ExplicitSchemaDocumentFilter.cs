// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace eppeta.webapi.Swagger
{
    public class ExplicitSchemaDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            // Don't know why this is showing up in the list of available schemas;
            // manually remove it.
            _ = context.SchemaRepository.Schemas.Remove("System.Text.Json.JsonValueKind");
        }
    }
}
