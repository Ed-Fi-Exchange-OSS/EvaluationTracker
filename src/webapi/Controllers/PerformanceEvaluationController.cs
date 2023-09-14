// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

using EdFi.OdsApi.Sdk.Apis.All;
using Microsoft.AspNetCore.Mvc;
using EdFi.OdsApi.Sdk.Client;
using eppeta.webapi.Service;
using EdFi.OdsApi.SdkClient;
using EdFi.OdsApi.Sdk.Models.All;

namespace webapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PerformanceEvaluationController : ControllerBase
{
    private readonly IODSAPIAuthenticationConfigurationService _service;
    public PerformanceEvaluationController(IODSAPIAuthenticationConfigurationService service)
    {
        _service = service;
    }

    [HttpGet("configuration")]
    public Configuration GetAuthenticatedConfiguration()
    {
        var authenticatedConfiguration = _service.GetAuthenticatedConfiguration();
        return authenticatedConfiguration;
    }

    // GET: api/EvaluationApi
    // The GetEvaluation method is slow due to the need to retrieve configuration first
    // TODO: Get the evaluation elements in the same method as GetEvaluation()
    [HttpGet]
    public async Task<ActionResult<Dictionary<string, List<string>>>> GetPerformanceEvaluations()
    {
        try
        {
            // Get ODS/API token
            var authenticatedConfiguration = _service.GetAuthenticatedConfiguration();

            // Get Evaluation Objectives
            var peApi = new PerformanceEvaluationsApi(authenticatedConfiguration);
            peApi.Configuration.DefaultHeaders.Add("Content-Type", "application/json");
            var performanceEvaluations = await peApi.GetPerformanceEvaluationsAsync(limit: 25, offset: 0);


            // Create a dictionary of EvaluationObjectiveTitles and EvaluationElementTitles
            //var performanceEvaluationsList = new List<TpdmPerformanceEvaluation>();
         
            return Ok(performanceEvaluations);
        }

        // temporary for debugging
        // TODO: Remove this catch block and add logging middleware EPPETA-25
        catch (Exception ex)
        {
#pragma warning disable CA2200 // Rethrow to preserve stack details
            throw ex;
#pragma warning restore CA2200 // Rethrow to preserve stack details
        }
    }

}
