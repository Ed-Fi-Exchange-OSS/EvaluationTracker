// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

using eppeta.webapi.Evaluations.Data;
using eppeta.webapi.Evaluations.Models;
using eppeta.webapi.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using eppeta.webapi.Identity.Models;
using OpenIddict.Abstractions;
using eppeta.webapi.Mapping;
using PerformanceEvaluationRating = eppeta.webapi.Evaluations.Models.PerformanceEvaluationRating;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace eppeta.webapi.Controllers
{
    //    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class EvaluationRatingController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEvaluationRepository _evaluationRepository;
        private readonly IOpenIddictTokenManager _tokenManager;

        public EvaluationRatingController(UserManager<ApplicationUser> userManager, IEvaluationRepository evaluationRepository, IOpenIddictTokenManager tokenManager)
        {
            _userManager = userManager;
            _evaluationRepository = evaluationRepository;
            _tokenManager = tokenManager;
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<PerformanceEvaluationRating>>> GetPerformanceEvaluationRatings()
        {
            var ratings = await _evaluationRepository.GetAllPerformanceEvaluationRatings();

            return Ok(ratings);
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<PerformanceEvaluationRating>>> GetPerformanceEvaluationRating([FromRoute] string userId)
        {
            if (userId is null || userId == string.Empty) { return NotFound(); }
            var ratingEntities = await _evaluationRepository.GetPerformanceEvalationRatingsByUserId(userId);

            if (ratingEntities == null)
            {
                return NotFound();
            }

            var ratingDTOs = ratingEntities.Select(rating => rating.ToRatingDTO(_userManager.FindByIdAsync(userId).Result));

            return Ok(ratingDTOs);
        }
    }
}
