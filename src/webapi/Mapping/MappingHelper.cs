
using eppeta.webapi.DTO;
using eppeta.webapi.Evaluations.Models;
using eppeta.webapi.Identity.Models;
using System.Runtime.CompilerServices;

namespace eppeta.webapi.Mapping
{
    public static class MappingHelper
    {
        public static PerformedEvaluation ToRatingDTO(this EvaluationRating ratingEntity, ApplicationUser user)
        {
            var evaluationDTO = new DTO.PerformedEvaluation
            {
                PerformanceEvaluationTitle = ratingEntity.PerformanceEvaluationTitle,
                ActualDate = ratingEntity.EvaluationDate,
                EvaluatorName = $"{user.FirstName} {user.LastName}",
                ReviewedCandidateName = ratingEntity.CandidateName,
                PerformanceEvaluationRatingId = ratingEntity.Id,
                ReviewedPersonId = ratingEntity.PersonId,
                ReviewedPersonIdSourceSystemDescriptor = ratingEntity.SourceSystemDescriptor
            };

            return evaluationDTO;
        }

        public static void PopulateEvaluationPK(object srcEvaluationObject, object dstEvaluationObject)
        {
            string[] evalPKCols =
                {
                "EducationOrganizationId",
                "EvaluationPeriodDescriptor",
                "EvaluationTitle",
                "PerformanceEvaluationTitle",
                "PerformanceEvaluationTypeDescriptor",
                "SchoolYear",
                "TermDescriptor"
            };
            var srcObjType = srcEvaluationObject.GetType();
            var dstObjType = dstEvaluationObject.GetType();
            foreach (var pkCol in evalPKCols)
            {
                dstObjType.GetProperty(pkCol).SetValue(
                    dstEvaluationObject,
                    srcObjType.GetProperty(pkCol).GetValue(srcEvaluationObject));
            }
        }
    }
}
