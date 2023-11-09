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
    public class EvaluationRating
    {
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
        public string? EvaluationRatingLevelDescriptor { get; set; } 
        public string? EvaluationRatingStatusDescriptor { get; set; } 
        public DateTime CreateDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string CandidateName { get; set; } = string.Empty;  
        [Required]
        [Column("EdFi_Id")]
        [StringLength(50)]
        public string EdFiId { get; set; }= string.Empty;
        public string UserId { get; set; } = string.Empty;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        // Foreign keys
        [ForeignKey("UserId")]
        public ApplicationUser? ApplicationUser { get; set; }
        public static explicit operator TpdmEvaluationRating(EvaluationRating evaluationRating)
        {
            return new TpdmEvaluationRating
            (
                performanceEvaluationRatingReference : new TpdmPerformanceEvaluationRatingReference
                (
                    educationOrganizationId : (int)evaluationRating.EducationOrganizationId,
                    evaluationPeriodDescriptor : evaluationRating.EvaluationPeriodDescriptor,
                    performanceEvaluationTitle : evaluationRating.PerformanceEvaluationTitle,
                    performanceEvaluationTypeDescriptor : evaluationRating.PerformanceEvaluationTypeDescriptor,
                    personId : evaluationRating.PersonId,
                    sourceSystemDescriptor : evaluationRating.SourceSystemDescriptor,
                    schoolYear : evaluationRating.SchoolYear,
                    termDescriptor : evaluationRating.TermDescriptor
                ),
                evaluationReference : new TpdmEvaluationReference
                (
                    educationOrganizationId : (int)evaluationRating.EducationOrganizationId,
                    evaluationPeriodDescriptor : evaluationRating.EvaluationPeriodDescriptor,
                    evaluationTitle : evaluationRating.EvaluationTitle,
                    performanceEvaluationTitle : evaluationRating.PerformanceEvaluationTitle,
                    performanceEvaluationTypeDescriptor : evaluationRating.PerformanceEvaluationTypeDescriptor,
                    schoolYear : evaluationRating.SchoolYear,
                    termDescriptor : evaluationRating.TermDescriptor
                ),
                evaluationDate : evaluationRating.EvaluationDate,
                evaluationRatingLevelDescriptor : evaluationRating.EvaluationRatingLevelDescriptor,
                evaluationRatingStatusDescriptor : evaluationRating.EvaluationRatingStatusDescriptor
            );
        }
    }
}

