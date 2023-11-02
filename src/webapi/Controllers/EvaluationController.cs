// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

using EdFi.OdsApi.Sdk.Apis.All;
using Microsoft.AspNetCore.Mvc;
using EdFi.OdsApi.Sdk.Client;
using eppeta.webapi.Service;
using EdFi.OdsApi.SdkClient;
using eppeta.webapi.Evaluations.Data;
using eppeta.webapi.Evaluations.Models;
using EdFi.OdsApi.Sdk.Models.All;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using eppeta.webapi.DTO;
using System.Linq.Dynamic.Core;

namespace webapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EvaluationController : ControllerBase
{
    private readonly IODSAPIAuthenticationConfigurationService _service;
    private readonly IEvaluationRepository _evaluationRepository;
    private readonly IMemoryCache _memoryCache;
    private string dataExpirationKey = "DataExpiration";
    private TimeSpan dataExpirationInterval = TimeSpan.FromDays(1);

    public EvaluationController(IODSAPIAuthenticationConfigurationService service, IEvaluationRepository evaluationRepository, IMemoryCache memoryCache)
    {
        _service = service;
        _evaluationRepository = evaluationRepository;
        _memoryCache = memoryCache;
    }

    [HttpGet("configuration")]
    public Configuration GetAuthenticatedConfiguration()
    {
        var authenticatedConfiguration = _service.GetAuthenticatedConfiguration();
        return authenticatedConfiguration;
    }

    [HttpGet("{performanceEvaluationId}")]
    public async Task<ActionResult<List<AvailablePerformanceEvaluation>>> GetEvaluationObjectivesElementsTitles(int performanceEvaluationId)
    {
        if (performanceEvaluationId == 0)
            return BadRequest();
        // use dynamic Linq to join on matching column names
        string[] nonJoinCols = { "EdFiId", "Id", "CreateDate", "LastModifiedDate" };
        // get performanceEvaluation, evaluationObjective and evaluationElements records, columns and matching columns
        var performanceEvaluation = new List<PerformanceEvaluation> { await _evaluationRepository.GetPerformanceEvaluationById(performanceEvaluationId) };
        if (!performanceEvaluation.Any())
            return NotFound();
        var evaluationObjectives = await _evaluationRepository.GetAllEvaluationObjectives();
        var evaluationElements = await _evaluationRepository.GetAllEvaluationElements();
        var performanceEvaluationCols = typeof(PerformanceEvaluation).GetProperties().Select(f => f.Name).ToList();
        var evaluationObjectivesCols = typeof(EvaluationObjective).GetProperties().Select(f => f.Name).ToList();
        var evaluationElementsCols = typeof(EvaluationElement).GetProperties().Select(f => f.Name).ToList();
        var matchingEvObjCols = performanceEvaluationCols.Intersect(evaluationObjectivesCols).Except(nonJoinCols).ToArray();
        var matchingObjElCols = evaluationObjectivesCols.Intersect(evaluationElementsCols).Except(nonJoinCols).ToArray();
        // build selectors: comma separated list of common column names
        string colEvObjSelector = "new {" + string.Join(",", matchingEvObjCols) + "}";
        string colObjElSelector = "new {" + string.Join(",", matchingObjElCols) + "}";
        // join performanceEvaluation with evaluationObjectives
        var filteredObjectives = performanceEvaluation.AsQueryable().Join(
            evaluationObjectives.AsQueryable(), colEvObjSelector, colEvObjSelector,
            "inner").ToDynamicList().OfType<EvaluationObjective>().ToList();

        var availableObjectives = filteredObjectives.AsQueryable().GroupJoin(
            evaluationElements.AsQueryable(), colObjElSelector, colObjElSelector,
            "new (outer.Id, outer.EvaluationObjectiveTitle, inner as elements)").ToDynamicList();
        var availablePerformanceEvaluation = new AvailablePerformanceEvaluation();
        availablePerformanceEvaluation.PerformanceEvaluationId = performanceEvaluationId;
        foreach (var availableObjective in availableObjectives)
        {
            AvailablePerformanceEvaluation.AvailableEvaluationObjective naeo = new AvailablePerformanceEvaluation.AvailableEvaluationObjective
            {
                Name = availableObjective.EvaluationObjectiveTitle,
                EvaluationObjectiveId = availableObjective.Id
            };
            foreach (EvaluationElement el in availableObjective.elements)
            {
                naeo.EvaluationElements.Add(new AvailablePerformanceEvaluation.AvailableEvaluationElement
                {
                    Name = el.EvaluationElementTitle,
                    EvaluationElementId = el.Id
                });
            }
            availablePerformanceEvaluation.EvaluationObjectives.Add(naeo);
        }
        availablePerformanceEvaluation.RatingLevels.AddRange(performanceEvaluation.First()
                .PerformanceEvaluationRatingLevels.Select(l => new AvailablePerformanceEvaluation.AvailableRatingLevel
                {
                    Name = l.EvaluationRatingLevelDescriptor.Split('#').Last(),
                    RatingLevel = (int)l.MaxRating,
                    RatingLevelId = l.Id
                }
            ));
        return Ok(availablePerformanceEvaluation);
    }
    // GET: api/EvaluationApi
    // The GetEvaluation method is slow due to the need to retrieve configuration first
    // TODO: Get the performanceEvaluation elements in the same method as GetEvaluation()
    [HttpGet("Sync")]
    public async Task<ActionResult> SyncEvaluationComponents()
    {
        try
        {
            // Check if already synced dependecies in cache
            if (_memoryCache.Get(dataExpirationKey) == null)
            {
                // Refresh Evaluation data from API
                // Get ODS/API token
                var authenticatedConfiguration = _service.GetAuthenticatedConfiguration();

                //// Get Evaluation Objectives and update repository
                var objectivesApi = new EvaluationObjectivesApi(authenticatedConfiguration);
                objectivesApi.Configuration.DefaultHeaders.Add("Content-Type", "application/json");
                var tpdmEvaluationObjectives = await objectivesApi.GetEvaluationObjectivesAsync(limit: 100, offset: 0);
                await _evaluationRepository.UpdateEvaluationObjectives(tpdmEvaluationObjectives.Select(teo => (EvaluationObjective)teo).ToList());

                // Get Evaluation Elements which contain the EvaluationObjectiveTitles and update repository
                var elementsApi = new EvaluationElementsApi(authenticatedConfiguration);
                elementsApi.Configuration.DefaultHeaders.Add("Content-Type", "application/json");
                var tpdmEvaluationElements = await elementsApi.GetEvaluationElementsAsync(limit: 100, offset: 0);
                await _evaluationRepository.UpdateEvaluationElements(tpdmEvaluationElements.Select(tee => (EvaluationElement)tee).ToList());

                var peApi = new PerformanceEvaluationsApi(authenticatedConfiguration);
                peApi.Configuration.DefaultHeaders.Add("Content-Type", "application/json");
                var tpdmPerformanceEvaluations = await peApi.GetPerformanceEvaluationsAsync(limit: 100, offset: 0);
                var performanceEvaluations = tpdmPerformanceEvaluations.Select(pe => (PerformanceEvaluation)pe).ToList();
                await _evaluationRepository.UpdatePerformanceEvaluations(performanceEvaluations);
                // set next expiration time
                var cachedValue = _memoryCache.GetOrCreate(
                    dataExpirationKey,
                    cacheEntry =>
                    {
                        cacheEntry.AbsoluteExpirationRelativeToNow = dataExpirationInterval;
                        return DateTime.Now;
                    });
            }
            return Ok();
        }

        // temporary for debugging
        // TODO: Remove this catch block and add logging middleware EPPETA-25
        catch (Exception ex)
        {
#pragma warning disable CA2200 // Rethrow to preserve stack details
            return Problem(ex.Message);
#pragma warning restore CA2200 // Rethrow to preserve stack details
        }
    }

}
