// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.


using eppeta.webapi.DTO;
using eppeta.webapi.Evaluations.Models;
using eppeta.webapi.Identity.Models;

namespace eppeta.webapi.Mapping
{
    public static class MappingHelper
    {
        public static PerformedEvaluation ToRatingDTO(this EvaluationRating ratingEntity, ApplicationUser user)
        {
            if (user is null) throw new ArgumentNullException(nameof(user));

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
            if (srcEvaluationObject is null) throw new ArgumentNullException(nameof(srcEvaluationObject));
            if (dstEvaluationObject is null) throw new ArgumentNullException(nameof(dstEvaluationObject));

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

            var dstProperties = dstObjType.GetProperties().ToList();
            var srcProperties = srcObjType.GetProperties().ToList();
            foreach (var pkCol in evalPKCols)
            {
                if (dstProperties.Exists(x => x.Name == pkCol) && srcProperties.Exists(x => x.Name == pkCol))
                {
                    // Disabled: the filters above solve this.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    dstObjType.GetProperty(pkCol).SetValue(
                        dstEvaluationObject,
                        srcObjType.GetProperty(pkCol).GetValue(srcEvaluationObject));
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                }
            }
        }
    }
}
