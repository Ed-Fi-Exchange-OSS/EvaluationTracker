// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

using EdFi.OdsApi.Sdk.Models.All;
using eppeta.webapi.Identity.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eppeta.webapi.Evaluations.Models
{
    public class EvaluationObjectiveRating
    {
        [Required]
        public string EvaluationObjectiveTitle { get; set; } = string.Empty;
        [Required]
        public long EducationOrganizationId { get; set; }
        [Required]
        public DateTime EvaluationDate { get; set; }
        [Required]
        public string EvaluationPeriodDescriptor { get; set; } = string.Empty;
        [Required]
        [StringLength(50)]
        public string EvaluationTitle { get; set; } = string.Empty;
        [Required]
        [StringLength(50)]
        public string PerformanceEvaluationTitle { get; set; } = string.Empty;
        [Required]
        public string PerformanceEvaluationTypeDescriptor { get; set; } = string.Empty;
        [Required]
        [StringLength(32)]
        public string PersonId { get; set; } = string.Empty;
        [Required]
        public short SchoolYear { get; set; }
        [Required]
        public string SourceSystemDescriptor { get; set; } = string.Empty;
        [Required]
        public string TermDescriptor { get; set; } = string.Empty;
        public string? ObjectiveRatingLevelDescriptor { get; set; }
        public string? Comments { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        [Required]
        [Column("EdFi_Id")]
        [StringLength(50)]
        public string EdFiId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        // Foreign keys
        [ForeignKey("UserId")]
        public ApplicationUser? ApplicationUser { get; set; }

        public static explicit operator TpdmEvaluationObjectiveRating(EvaluationObjectiveRating evaluationObjectiveRating)
        {
            return new TpdmEvaluationObjectiveRating
            (
                evaluationObjectiveReference: new TpdmEvaluationObjectiveReference
                (
                    educationOrganizationId: (int)evaluationObjectiveRating.EducationOrganizationId,
                    evaluationObjectiveTitle: evaluationObjectiveRating.EvaluationObjectiveTitle,
                    evaluationPeriodDescriptor: evaluationObjectiveRating.EvaluationPeriodDescriptor,
                    evaluationTitle: evaluationObjectiveRating.EvaluationTitle,
                    performanceEvaluationTitle: evaluationObjectiveRating.PerformanceEvaluationTitle,
                    performanceEvaluationTypeDescriptor: evaluationObjectiveRating.PerformanceEvaluationTypeDescriptor,
                    schoolYear: evaluationObjectiveRating.SchoolYear,
                    termDescriptor: evaluationObjectiveRating.TermDescriptor
                ),
                evaluationRatingReference: new TpdmEvaluationRatingReference
                (
                    educationOrganizationId: (int)evaluationObjectiveRating.EducationOrganizationId,
                    evaluationPeriodDescriptor: evaluationObjectiveRating.EvaluationPeriodDescriptor,
                    evaluationTitle: evaluationObjectiveRating.EvaluationTitle,
                    performanceEvaluationTitle: evaluationObjectiveRating.PerformanceEvaluationTitle,
                    performanceEvaluationTypeDescriptor: evaluationObjectiveRating.PerformanceEvaluationTypeDescriptor,
                    evaluationDate: evaluationObjectiveRating.EvaluationDate,
                    schoolYear: evaluationObjectiveRating.SchoolYear,
                    termDescriptor: evaluationObjectiveRating.TermDescriptor,
                    personId: evaluationObjectiveRating.PersonId,
                    sourceSystemDescriptor: evaluationObjectiveRating.SourceSystemDescriptor
                ),
                objectiveRatingLevelDescriptor: evaluationObjectiveRating.ObjectiveRatingLevelDescriptor ?? string.Empty,
                comments: evaluationObjectiveRating.Comments ?? string.Empty
            );
        }

        public static explicit operator TpdmEvaluationRating(EvaluationObjectiveRating evaluationObjectiveRating)
        {
            return new TpdmEvaluationRating
            (
                performanceEvaluationRatingReference: new TpdmPerformanceEvaluationRatingReference
                (
                    educationOrganizationId: (int)evaluationObjectiveRating.EducationOrganizationId,
                    evaluationPeriodDescriptor: evaluationObjectiveRating.EvaluationPeriodDescriptor,
                    performanceEvaluationTitle: evaluationObjectiveRating.PerformanceEvaluationTitle,
                    performanceEvaluationTypeDescriptor: evaluationObjectiveRating.PerformanceEvaluationTypeDescriptor,
                    schoolYear: evaluationObjectiveRating.SchoolYear,
                    termDescriptor: evaluationObjectiveRating.TermDescriptor,
                    personId: evaluationObjectiveRating.PersonId,
                    sourceSystemDescriptor: evaluationObjectiveRating.SourceSystemDescriptor
                ),
                evaluationReference: new TpdmEvaluationReference
                (
                    educationOrganizationId: (int)evaluationObjectiveRating.EducationOrganizationId,
                    evaluationPeriodDescriptor: evaluationObjectiveRating.EvaluationPeriodDescriptor,
                    evaluationTitle: evaluationObjectiveRating.EvaluationTitle,
                    performanceEvaluationTitle: evaluationObjectiveRating.PerformanceEvaluationTitle,
                    performanceEvaluationTypeDescriptor: evaluationObjectiveRating.PerformanceEvaluationTypeDescriptor,
                    schoolYear: evaluationObjectiveRating.SchoolYear,
                    termDescriptor: evaluationObjectiveRating.TermDescriptor
                ),
                evaluationDate: evaluationObjectiveRating.EvaluationDate
            );
        }

        public static explicit operator TpdmEvaluation(EvaluationObjectiveRating evaluationObjectiveRating)
        {
            return new TpdmEvaluation
            (
                performanceEvaluationReference: new TpdmPerformanceEvaluationReference
                (
                    educationOrganizationId: (int)evaluationObjectiveRating.EducationOrganizationId,
                    evaluationPeriodDescriptor: evaluationObjectiveRating.EvaluationPeriodDescriptor,
                    performanceEvaluationTitle: evaluationObjectiveRating.PerformanceEvaluationTitle,
                    performanceEvaluationTypeDescriptor: evaluationObjectiveRating.PerformanceEvaluationTypeDescriptor,
                    schoolYear: evaluationObjectiveRating.SchoolYear,
                    termDescriptor: evaluationObjectiveRating.TermDescriptor
                ),
                evaluationTitle: evaluationObjectiveRating.EvaluationTitle
            );
        }
    }
}

