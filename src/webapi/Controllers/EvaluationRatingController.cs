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
        public async Task<ActionResult<IEnumerable<PerformedEvaluation>>> GetPerformedEvaluations()
        {
            var ratings = await _evaluationRepository.GetAllPerformanceEvaluationRatings();

            return Ok(ratings);
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<PerformedEvaluation>>> GetPerformedEvaluations([FromRoute] string userId)
        {
            if (userId is null || userId == string.Empty) { return NotFound(); }
            var ratingEntities = await _evaluationRepository.GetEvaluationRatingsByUserId(userId);

            if (ratingEntities == null)
            {
                return NotFound();
            }

            var ratingDTOs = ratingEntities.Select(rating => rating.ToRatingDTO(_userManager.FindByIdAsync(userId).Result));

            return Ok(ratingDTOs);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(PerformedEvaluationResult evaluationResult, string userId)
        {
            try
            {
                if (evaluationResult == null)
                    return BadRequest();
                foreach (var objRes in evaluationResult.ObjectiveResults)
                {
                    // Create EvaluationRating, EvaluationObjectiveRating
                    var evalObjective = await _evaluationRepository.GetEvaluationObjectiveById(objRes.Id);
                    var user = await _userManager.FindByIdAsync(userId);
                    EvaluationRating newEvalRating = new EvaluationRating();
                    EvaluationObjectiveRating newObjRating = new EvaluationObjectiveRating();
                    MappingHelper.PopulateEvaluationPK(newEvalRating, evalObjective);
                    MappingHelper.PopulateEvaluationPK(newObjRating, evalObjective);
                    newEvalRating.EvaluationDate = newObjRating.EvaluationDate = evaluationResult.StartDateTime;
                    newEvalRating.PersonId = newObjRating.PersonId = evaluationResult.ReviewedPersonId;
                    newEvalRating.SourceSystemDescriptor = newObjRating.SourceSystemDescriptor = evaluationResult.ReviewedPersonSourceSystemDescriptor;
                    newEvalRating.UserId = newObjRating.UserId = user.Id;
                    newObjRating.Comments = objRes.Comment;
                    await _evaluationRepository.UpdateEvaluationRatings(new List<EvaluationRating> { newEvalRating });
                    await _evaluationRepository.UpdateEvaluationObjectiveRatings(new List<EvaluationObjectiveRating> { newObjRating });
                    foreach (var elRes in objRes.Elements)
                    {
                        var evalElement = await _evaluationRepository.GetEvaluationElementById(elRes.Id);
                        EvaluationElementRatingResult elementRatingResult = new EvaluationElementRatingResult();
                        MappingHelper.PopulateEvaluationPK(evalElement, elementRatingResult);
                        elementRatingResult.EvaluationElementTitle = evalElement.EvaluationElementTitle;
                        elementRatingResult.EvaluationObjectiveTitle = evalElement.EvaluationObjectiveTitle;
                        elementRatingResult.PersonId = newObjRating.PersonId;
                        elementRatingResult.SourceSystemDescriptor = newObjRating.SourceSystemDescriptor;
                        elementRatingResult.RatingResultTitle = evalElement.EvaluationElementTitle;
                        elementRatingResult.Rating = elRes.Score;
                        elementRatingResult.UserId = user.Id;
                        await _evaluationRepository.UpdateEvaluationElementRatingResults(new List<EvaluationElementRatingResult> { elementRatingResult });
                    }
                }
                return Ok();
            }catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
