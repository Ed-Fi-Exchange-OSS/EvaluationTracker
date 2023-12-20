// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

using Microsoft.AspNetCore.Mvc;
using eppeta.webapi.Service;
using eppeta.webapi.Evaluations.Data;
using eppeta.webapi.DTO;
using eppeta.webapi.Evaluations.Models;

namespace webapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PerformanceEvaluationController : ControllerBase
{
    private readonly IODSAPIAuthenticationConfigurationService _service;
    private readonly IEvaluationRepository _evaluationRepository;

    public PerformanceEvaluationController(IODSAPIAuthenticationConfigurationService service, IEvaluationRepository evaluationRepository)
    {
        _service = service;
        _evaluationRepository = evaluationRepository;
    }

    [HttpGet("configuration")]
    public async Task<ActionResult> GetAuthenticatedConfiguration()
    {
        var authenticatedConfiguration = await _service.GetAuthenticatedConfiguration();
        return Ok(authenticatedConfiguration);
    }

    // GET: api/EvaluationApi
    // The GetEvaluation method is slow due to the need to retrieve configuration first
    // TODO: Get the evaluation elements in the same method as GetEvaluation()
    [HttpGet]
    public async Task<ActionResult<Dictionary<string, List<string>>>> GetPerformanceEvaluations()
    {
        var performanceEvaluations = await _evaluationRepository.GetAllPerformanceEvaluations();
        return Ok(performanceEvaluations);
    }

    [HttpGet("{PerformanceEvaluationRatingId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PerformedEvaluationResult>> GetPerformedEvaluation(int PerformanceEvaluationRatingId)
    {
        var evaluationRating = await _evaluationRepository.GetEvaluationRatingById(PerformanceEvaluationRatingId);
        var perEvalRatingDB = (await _evaluationRepository.GetPerformanceEvaluationRatingsByPK(evaluationRating)).FirstOrDefault();
        if (evaluationRating == null || perEvalRatingDB == null)
            return NotFound();

        var performedEvaluation = new PerformedEvaluationResult {
            EvaluationRatingId = evaluationRating.Id,
            ReviewedCandidateName = perEvalRatingDB.ReviewedCandidateName ?? string.Empty,
            ReviewedPersonId = evaluationRating.PersonId,
            ReviewedPersonSourceSystemDescriptor = evaluationRating.SourceSystemDescriptor,
            StartDateTime = perEvalRatingDB.StartTime,
            EndDateTime = perEvalRatingDB.EndTime,
            EvaluatorName = perEvalRatingDB.EvaluatorName,
            StatusId = perEvalRatingDB.StatusId,
            PerformanceEvaluationTitle = evaluationRating.PerformanceEvaluationTitle,
            UserId = evaluationRating.UserId,
            ObjectiveResults = new List<PerformedEvaluationResult.PerformedEvaluationResultObjective>()
        };

        var evaluation = await _evaluationRepository.GetPerformanceEvaluationByPK(evaluationRating);
        if (evaluation != null)
            performedEvaluation.EvaluationId = evaluation.First().Id;
        
        var evaluationObjectiveRatings = await _evaluationRepository.GetEvaluationObjectiveRatingsByPK(evaluationRating);
        if (evaluationObjectiveRatings != null)
        {
            foreach (var evaluationObjectiveRating in evaluationObjectiveRatings)
            {
                var evaluationObjective = _evaluationRepository.GetEvaluationObjectivesByPK(evaluationObjectiveRating).Result.FirstOrDefault();
                var objectiveResult = new PerformedEvaluationResult.PerformedEvaluationResultObjective
                {
                    Id = evaluationObjective.Id,
                    Comment = evaluationObjectiveRating.Comments,
                    Elements = new List<PerformedEvaluationResult.PerformedEvaluationResultElement>()
                };
                var evaluationElementRatings = await _evaluationRepository.GetEvaluationElementRatingResultsByPK(evaluationObjectiveRating);
                if (evaluationElementRatings != null)
                {
                    foreach (var evaluationElementRating in evaluationElementRatings)
                    {
                        var evaluationElement = _evaluationRepository.GetEvaluationElementsByPK(evaluationElementRating).Result;
                        objectiveResult.Elements.Add(new PerformedEvaluationResult.PerformedEvaluationResultElement
                        {
                            Score = (int)evaluationElementRating.Rating,
                            Id = evaluationElement.FirstOrDefault().Id,
                        });
                    }
                }
                performedEvaluation.ObjectiveResults.Add(objectiveResult);
            }
        }
        return Ok(performedEvaluation);
    }
}
