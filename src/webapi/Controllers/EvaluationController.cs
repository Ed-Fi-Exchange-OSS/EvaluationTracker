// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

using eppeta.webapi.DTO;
using eppeta.webapi.Evaluations.Data;
using eppeta.webapi.Evaluations.Models;
using eppeta.webapi.Identity.Models;
using eppeta.webapi.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using OpenIddict.Validation.AspNetCore;
using System.Linq.Dynamic.Core;

namespace eppeta.webapi.Controllers;

[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Roles = $"{Roles.Reviewer}, {Roles.Evaluator}")]
[ApiController]
[Route("api/[controller]")]
public class EvaluationController : ControllerBase
{
    private readonly IODSAPIAuthenticationConfigurationService _service;
    private readonly IEvaluationRepository _evaluationRepository;
    private readonly IMemoryCache _memoryCache;
    private const string dataExpirationKey = "DataExpiration";
    private readonly TimeSpan dataExpirationInterval = TimeSpan.FromDays(1);

    public EvaluationController(IODSAPIAuthenticationConfigurationService service, IEvaluationRepository evaluationRepository, IMemoryCache memoryCache)
    {
        _service = service;
        _evaluationRepository = evaluationRepository;
        _memoryCache = memoryCache;
    }

    [HttpGet("configuration")]
    public async Task<ActionResult> GetAuthenticatedConfiguration()
    {
        var authenticatedConfiguration = await _service.GetAuthenticatedConfiguration();
        return Ok(authenticatedConfiguration);
    }

    [HttpGet("EvaluationStatuses")]
    public async Task<ActionResult<List<Status>>> GetEvaluationStatuses()
    {
        var statuses = await _evaluationRepository.GetAllStatuses();
        return statuses;
    }

    [HttpGet("{performanceEvaluationId}")]
    public async Task<ActionResult<List<AvailablePerformanceEvaluation>>> GetEvaluationObjectivesElementsTitles(int performanceEvaluationId)
    {
        if (performanceEvaluationId == 0)
        {
            return BadRequest();
        }
        // use dynamic Linq to join on matching column names
        string[] nonJoinCols = { "EdFiId", "Id", "CreateDate", "LastModifiedDate" };
        // get performanceEvaluation, evaluationObjective and evaluationElements records, columns and matching columns
        var performanceEvaluation = new List<PerformanceEvaluation> { await _evaluationRepository.GetPerformanceEvaluationById(performanceEvaluationId) };
        if (!performanceEvaluation.Any() || performanceEvaluation[0] == null)
        {
            return NotFound();
        }

        var evaluationObjectives = await _evaluationRepository.GetAllEvaluationObjectives();
        var evaluationElements = await _evaluationRepository.GetAllEvaluationElements();
        var performanceEvaluationCols = typeof(PerformanceEvaluation).GetProperties().Select(f => f.Name).ToList();
        var evaluationObjectivesCols = typeof(EvaluationObjective).GetProperties().Select(f => f.Name).ToList();
        var evaluationElementsCols = typeof(EvaluationElement).GetProperties().Select(f => f.Name).ToList();
        var matchingEvObjCols = performanceEvaluationCols.Intersect(evaluationObjectivesCols).Except(nonJoinCols).ToArray();
        var matchingObjElCols = evaluationObjectivesCols.Intersect(evaluationElementsCols).Except(nonJoinCols).ToArray();
        // build selectors: comma separated list of common column names
        var colEvObjSelector = "new {" + string.Join(",", matchingEvObjCols) + "}";
        var colObjElSelector = "new {" + string.Join(",", matchingObjElCols) + "}";
        // join performanceEvaluation with evaluationObjectives
        var filteredObjectives = performanceEvaluation.AsQueryable().Join(
            evaluationObjectives.AsQueryable(), colEvObjSelector, colEvObjSelector,
            "inner").ToDynamicList().OfType<EvaluationObjective>().ToList();

        var availableObjectives = filteredObjectives.AsQueryable().GroupJoin(
            evaluationElements.AsQueryable(), colObjElSelector, colObjElSelector,
            "new (outer.Id, outer.EvaluationObjectiveTitle, inner as elements)").ToDynamicList();
        var availablePerformanceEvaluation = new AvailablePerformanceEvaluation
        {
            PerformanceEvaluationId = performanceEvaluationId
        };
        foreach (var availableObjective in availableObjectives)
        {
            var naeo = new AvailablePerformanceEvaluation.AvailableEvaluationObjective
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
                    RatingLevel = (int)(l?.MaxRating ?? default(int)),
                    RatingLevelId = l?.Id ?? default
                }
            ));
        return Ok(availablePerformanceEvaluation);
    }
}
