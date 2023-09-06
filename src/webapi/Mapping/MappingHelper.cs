
using eppeta.webapi.DTO;
using eppeta.webapi.Identity.Models;
using System.Runtime.CompilerServices;

namespace eppeta.webapi.Mapping
{
    public static class MappingHelper
    {
        public static PerformanceEvaluationRating ToRatingDTO(this Evaluations.Models.PerformanceEvaluationRating ratingEntity, ApplicationUser user)
        {
            var evaluationDTO = new DTO.PerformanceEvaluationRating
            {
                PerformanceEvaluationTitle = ratingEntity.PerformanceEvaluationTitle,
                ActualDate = ratingEntity.ActualDate,
                EvaluatorName = $"{user.FirstName} {user.LastName}",
                ReviewedCandidateName = ratingEntity.ReviewedCandidateName
            };

            return evaluationDTO;
        }
    }
}
