// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

using eppeta.webapi.Identity.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eppeta.webapi.Evaluations.Models
{
    public class PerformanceEvaluationRating
    {
        [Required]
        public int EducationOrganizationId { get; set; }
        [Required]
        public string EvaluationPeriodDescriptor { get; set; } = string.Empty;
        [Required]
        public string PerformanceEvaluationTitle { get; set; } = string.Empty;
        [Required]
        public string PerformanceEvaluationTypeDescriptor { get; set; } = string.Empty;
        [Required]
        public short SchoolYear { get; set; }
        [Required]
        public string TermDescriptor { get; set; } = string.Empty;
        public DateTime ActualDate { get; set; }
        public int? ActualDuration { get; set; }
        public string? PerformanceEvaluationRatingLevelDescriptor { get; set; }
        [Required]
        [StringLength(32)]
        public string PersonId { get; set; } = string.Empty;
        [Required]
        public string SourceSystemDescriptor { get; set; } = string.Empty;
        public TimeSpan? ActualTime { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        [Column("EdFi_Id")]
        [StringLength(50)]
        public string EdFiId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public int StatusId { get; set; } = 1;
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Foreign keys
        [ForeignKey("UserId")]
        public ApplicationUser? ApplicationUser { get; set; }
        [ForeignKey("StatusId")]
        public Status? RecordStatus { get; set; }
    }
}

