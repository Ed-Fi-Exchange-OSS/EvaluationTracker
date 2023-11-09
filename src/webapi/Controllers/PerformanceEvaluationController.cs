// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

using Microsoft.AspNetCore.Mvc;
using eppeta.webapi.Service;
using eppeta.webapi.Evaluations.Data;

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

}
