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
    public class PerformanceEvaluationRating
    {
        [Required]
        public long EducationOrganizationId { get; set; }
        [Required]
        public string EvaluationPeriodDescriptor { get; set; } = string.Empty;
        [Required]
        public string PerformanceEvaluationTitle { get; set; } = string.Empty;
        [Required]
        public string PerformanceEvaluationTypeDescriptor { get; set; } = string.Empty;
        [Required]
        [StringLength(32)]
        public string PersonId { get; set; } = string.Empty;
        [Required]
        public string SourceSystemDescriptor { get; set; } = string.Empty;
        [Required]
        public short SchoolYear { get; set; }
        [Required]
        public string TermDescriptor { get; set; } = string.Empty;
        public string? PerformanceEvaluationRatingLevelDescriptor { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime CreateDate { get; set; }
        [Column("EdFi_Id")]
        [StringLength(50)]
        public string? EdFiId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int? StatusId { get; set; } = 1;
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Foreign keys
        [ForeignKey("UserId")]
        public ApplicationUser? ApplicationUser { get; set; }
        [ForeignKey("StatusId")]
        public Status? RecordStatus { get; set; }

        public static explicit operator TpdmPerformanceEvaluationRating(PerformanceEvaluationRating performanceEvaluationRating)
        => new TpdmPerformanceEvaluationRating
                (
                    performanceEvaluationReference: new TpdmPerformanceEvaluationReference
                    (
                        educationOrganizationId: (int)performanceEvaluationRating.EducationOrganizationId,
                        evaluationPeriodDescriptor: performanceEvaluationRating.EvaluationPeriodDescriptor,
                        performanceEvaluationTitle: performanceEvaluationRating.PerformanceEvaluationTitle,
                        performanceEvaluationTypeDescriptor: performanceEvaluationRating.PerformanceEvaluationTypeDescriptor,
                        schoolYear: performanceEvaluationRating.SchoolYear,
                        termDescriptor: performanceEvaluationRating.TermDescriptor
                    ),
                    personReference: new EdFiPersonReference
                    (
                        personId: performanceEvaluationRating.PersonId,
                        sourceSystemDescriptor: performanceEvaluationRating.SourceSystemDescriptor
                    ),
                    actualDate: performanceEvaluationRating.StartTime,
                    actualDuration: (performanceEvaluationRating.EndTime - performanceEvaluationRating.StartTime)?.Minutes,
                    performanceEvaluationRatingLevelDescriptor: performanceEvaluationRating.PerformanceEvaluationRatingLevelDescriptor
                );
    }
}

