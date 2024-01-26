// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

using eppeta.webapi.DTO;
using eppeta.webapi.Evaluations.Data;
using eppeta.webapi.Identity.Models;
using eppeta.webapi.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;

namespace eppeta.webapi.Controllers
{
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Roles = $"{Roles.Reviewer}, {Roles.Evaluator}")]
    [Route("api/[controller]")]
    [ApiController]
    public class CandidateController : ControllerBase
    {
        private readonly IEvaluationRepository _evaluationRepository;
        public CandidateController(IODSAPIAuthenticationConfigurationService service, IEvaluationRepository evaluationRepository)
        {
            _evaluationRepository = evaluationRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Candidate>>> GetCandidates()
        {
            var candidates = await _evaluationRepository.GetAllCandidates();
            if (candidates != null)
            {
                return Ok(candidates.Select(c => new Candidate()
                {
                    CandidateName = $"{c.FirstName} {c.LastName}",
                    PersonId = c.PersonId,
                    SourceSystemDescriptor = c.SourceSystemDescriptor
                }).ToList());
            }
            else
                return Ok(new List<Candidate>());
        }
    }
}
