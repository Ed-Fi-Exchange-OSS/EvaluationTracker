// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

using System.Net;
using System.Text.Json;

namespace eppeta.webapi.Infrastructure;

public class LoggingMiddleware
{
    static readonly Serilog.ILogger Log = Serilog.Log.ForContext<LoggingMiddleware>();
    private readonly RequestDelegate _next;

    public LoggingMiddleware(RequestDelegate next)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
    }

    public async Task Invoke(HttpContext context)
    {
        context = context ?? throw new ArgumentNullException(nameof(context));

        var requestInfo = new {
            method = context.Request.Method,
            path = context.Request.Path.Value,
            contentType = context.Request.ContentType,
            traceId = context.TraceIdentifier
        };
        try
        {
            if (context.Request.Path.StartsWithSegments(new PathString("/.well-known")))
            {
                // Requests to the OpenId Connect ".well-known" endpoint are too chatty for informational logging,
                // but could be useful in debug logging.
                Log.Debug("{@RequestInfo}", requestInfo);
            }
            else
            {
                Log.Information("{@RequestInfo}", requestInfo);
            }
            await _next(context);
        }
        catch (Exception ex)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            Log.Error("{@Error}", new { ex.Message, ex.StackTrace, traceId = context.TraceIdentifier });
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var responseMessage = new { message = "The server encountered an unexpected condition that prevented it from fulfilling the request.", traceId = context.TraceIdentifier };
            await response.WriteAsync(JsonSerializer.Serialize(responseMessage));
        }
    }
}
