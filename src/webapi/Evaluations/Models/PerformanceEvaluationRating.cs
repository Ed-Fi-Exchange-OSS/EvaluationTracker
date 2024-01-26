// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

using EdFi.OdsApi.Sdk.Models.All;
using eppeta.webapi.DTO;
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
        public string? Comments { get; set; }
        public string? ReviewedCandidateName { get; set; }
        public string? EvaluatorName { get; set; }
        public string? PerformanceEvaluationRatingLevelDescriptor { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        [NotMapped]
        public DateTime ActualDate { get; set; }
        [NotMapped]
        public int ActualDuration { get; set; }
        [NotMapped]
        public string? ActualTime { get; set; }
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
        {
            return new TpdmPerformanceEvaluationRating
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
                            actualDuration: (int)((performanceEvaluationRating.EndTime ?? DateTime.Now) - performanceEvaluationRating.StartTime).TotalMinutes,
                            // The API doesn't like a value here
                            //actualTime: TimeOnly.FromDateTime(performanceEvaluationRating.StartTime).ToShortTimeString(),
                            performanceEvaluationRatingLevelDescriptor: performanceEvaluationRating?.PerformanceEvaluationRatingLevelDescriptor ?? string.Empty,
                            comments: performanceEvaluationRating?.Comments ?? string.Empty
                        );
        }

        public static explicit operator PerformedEvaluation(PerformanceEvaluationRating performanceEvaluationRating)
        {
            return new PerformedEvaluation
            {
                ActualDate = performanceEvaluationRating.StartTime,
                EvaluationStatus = performanceEvaluationRating.RecordStatus != null ? performanceEvaluationRating.RecordStatus.StatusText : "Ready for Review",
                PerformanceEvaluationRatingId = performanceEvaluationRating.Id,
                PerformanceEvaluationTitle = performanceEvaluationRating.PerformanceEvaluationTitle,
                ReviewedPersonId = performanceEvaluationRating.PersonId,
                ReviewedPersonIdSourceSystemDescriptor = performanceEvaluationRating.SourceSystemDescriptor,
                EvaluatorName = performanceEvaluationRating.EvaluatorName,
                ReviewedCandidateName = performanceEvaluationRating.ReviewedCandidateName,
                Comments = performanceEvaluationRating.Comments
            };
        }
    }
}

