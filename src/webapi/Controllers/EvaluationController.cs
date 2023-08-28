// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

using EdFi.OdsApi.Sdk.Apis.All;
using Microsoft.AspNetCore.Mvc;
using EdFi.OdsApi.Sdk.Client;
using eppeta.webapi.Service;
using EdFi.OdsApi.SdkClient;

namespace webapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EvaluationController : ControllerBase
{
    private readonly IODSAPIAuthenticationConfigurationService _service;
    public EvaluationController(IODSAPIAuthenticationConfigurationService service)
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
    public async Task<ActionResult<Dictionary<string, List<string>>>> GetEvaluationObjectivesElementsTitles()
    {
        try
        {
            // Get ODS/API token
            var authenticatedConfiguration = _service.GetAuthenticatedConfiguration();
            
            //// Get Evaluation Objectives
            var objectivesApi = new EvaluationObjectivesApi(authenticatedConfiguration);
            objectivesApi.Configuration.DefaultHeaders.Add("Content-Type", "application/json");
            var evaluationObjectives = await objectivesApi.GetEvaluationObjectivesAsync(limit: 25, offset: 0);

            // Get Evaluation Elements which contain the EvaluationObjectiveTitles
            var elementsApi = new EvaluationElementsApi(authenticatedConfiguration);
            elementsApi.Configuration.DefaultHeaders.Add("Content-Type", "application/json");
            var evaluationElements = await elementsApi.GetEvaluationElementsAsync(limit: 25, offset: 0);

            // Create a dictionary of EvaluationObjectiveTitles and EvaluationElementTitles
            var evaluationElementsDictionary = new Dictionary<string, List<string>>();
            foreach (var element in evaluationElements)
            {
                var objectiveTitle = element.EvaluationObjectiveReference.EvaluationObjectiveTitle;
                if (!evaluationElementsDictionary.ContainsKey(objectiveTitle))
                {
                    evaluationElementsDictionary[objectiveTitle] = new List<string>();
                }
                evaluationElementsDictionary[objectiveTitle].Add(element.EvaluationElementTitle);
            }
            return Ok(evaluationElementsDictionary);
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
