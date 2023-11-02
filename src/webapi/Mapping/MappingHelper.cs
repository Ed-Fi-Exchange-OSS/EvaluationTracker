
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

        public static void CopyMatchingPKProperties(object srcObject, object dstObject)
        {
            string[] pkCols =
                {
                "EducationOrganizationId",
                "PerformanceEvaluationTitle",
                "PerformanceEvaluationTypeDescriptor",
                "SchoolYear",
                "TermDescriptor",
                "EvaluationTitle",
                "EvaluationPeriodDescriptor",
                "EvaluationDate",
                "EvaluationTypeDescriptor",
                "PersonId",
                "SourceSystemDescriptor",
                "EvaluationObjectiveTitle",
                "EvaluationElementTitle"
            };
            var srcObjType = srcObject.GetType();
            var dstObjType = dstObject.GetType();
            foreach (var pkCol in pkCols)
            {
                var srcProp = srcObjType.GetProperty(pkCol);
                if (srcProp == null)
                {
                    Console.WriteLine($"Property {pkCol} not found in object type {srcObject.GetType().Name}");
                    continue;
                }
                var dstProp = dstObjType.GetProperty(pkCol);
                if (dstProp == null)
                {
                    Console.WriteLine($"Property {pkCol} not found in object type {dstObject.GetType().Name}");
                    continue;
                }
                dstProp.SetValue(dstObject, srcProp.GetValue(srcObject));
            }
        }
    }
}
