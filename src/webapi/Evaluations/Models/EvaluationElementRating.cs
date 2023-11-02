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
    public class EvaluationElementRating
    {
        [Required]
        public long EducationOrganizationId { get; set; }
        public DateTime EvaluationDate { get; set; }
        [Required]
        [StringLength(255)]
        public string EvaluationElementTitle { get; set; }
        [Required]
        [StringLength(50)]
        public string EvaluationObjectiveTitle { get; set; }
        [Required]
        public string EvaluationPeriodDescriptor { get; set; }
        [Required]
        [StringLength(50)]
        public string EvaluationTitle { get; set; }
        [Required]
        [StringLength(50)]
        public string PerformanceEvaluationTitle { get; set; }
        [Required]
        public string PerformanceEvaluationTypeDescriptor { get; set; }
        [Required]
        [StringLength(32)]
        public string PersonId { get; set; }
        [Required]
        public short SchoolYear { get; set; }
        [Required]
        public string SourceSystemDescriptor { get; set; }
        [Required]
        public string TermDescriptor { get; set; }
        public string? EvaluationElementRatingLevelDescriptor { get; set; } = string.Empty;
        public decimal Rating { get; set; }
        public DateTime CreateDate { get; set; }
        public string? EdFi_Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Foreign keys
        [ForeignKey("UserId")]
        public ApplicationUser? ApplicationUser { get; set; }
        [ForeignKey("StatusId")]
        public Status? RecordStatus { get; set; }
    public static explicit operator TpdmEvaluationElementRating(EvaluationElementRating evaluationElementRating)
        => new TpdmEvaluationElementRating
        (
            evaluationElementReference: new TpdmEvaluationElementReference
            (
                educationOrganizationId: (int)evaluationElementRating.EducationOrganizationId,
                evaluationElementTitle:evaluationElementRating.EvaluationElementTitle,
                evaluationObjectiveTitle: evaluationElementRating.EvaluationObjectiveTitle,
                evaluationPeriodDescriptor: evaluationElementRating.EvaluationPeriodDescriptor,
                evaluationTitle: evaluationElementRating.EvaluationTitle,
                performanceEvaluationTitle: evaluationElementRating.PerformanceEvaluationTitle,
                performanceEvaluationTypeDescriptor: evaluationElementRating.PerformanceEvaluationTypeDescriptor,
                schoolYear: evaluationElementRating.SchoolYear,
                termDescriptor:evaluationElementRating.TermDescriptor
            ),
            evaluationObjectiveRatingReference: new TpdmEvaluationObjectiveRatingReference
            (
                educationOrganizationId: (int)evaluationElementRating.EducationOrganizationId,
                evaluationObjectiveTitle: evaluationElementRating.EvaluationObjectiveTitle,
                evaluationPeriodDescriptor: evaluationElementRating.EvaluationPeriodDescriptor,
                evaluationTitle: evaluationElementRating.EvaluationTitle,
                evaluationDate: evaluationElementRating.EvaluationDate,
                performanceEvaluationTitle: evaluationElementRating.PerformanceEvaluationTitle,
                performanceEvaluationTypeDescriptor: evaluationElementRating.PerformanceEvaluationTypeDescriptor,
                schoolYear: evaluationElementRating.SchoolYear,
                termDescriptor: evaluationElementRating.TermDescriptor,
                personId: evaluationElementRating.PersonId,
                sourceSystemDescriptor: evaluationElementRating.SourceSystemDescriptor
            )
        );
    }
}

