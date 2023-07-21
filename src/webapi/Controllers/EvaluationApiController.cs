// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EdFi.OdsApi.Sdk.Apis.All;
using EdFi.OdsApi.Sdk.Models.All;
using Microsoft.AspNetCore.Mvc;
using EdFi.OdsApi.Sdk.Client;
using EdFi.OdsApi.SdkClient;
using eppeta.webapi.Service;

namespace webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EvaluationApiController : ControllerBase
    {
        private readonly IAuthenticationConfigurationService _service;
        public EvaluationApiController(IAuthenticationConfigurationService service)
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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TpdmEvaluationObjective>>> GetEvaluation()
        {
            try
            {
                // Get the authenticated configuration using the injected service
                var authenticatedConfiguration = _service.GetAuthenticatedConfiguration();

                // Get Evaluation Objectives
                var apiInstance = new EvaluationObjectivesApi(authenticatedConfiguration);
                apiInstance.Configuration.DefaultHeaders.Add("Content-Type", "application/json");

                var evaluationObjectivesWithHttpInfo = apiInstance.GetEvaluationObjectivesWithHttpInfo(limit: 25, offset: 0, totalCount: true);

                var httpReponseCode = evaluationObjectivesWithHttpInfo.StatusCode; // returns System.Net.HttpStatusCode.OK
                Console.WriteLine("Response code is " + httpReponseCode);

                var evaluationObjectives = await apiInstance.GetEvaluationObjectivesAsync(limit: 25, offset: 0);

                // Return the evaluationObjectives directly, no need to serialize again
                return evaluationObjectives;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
