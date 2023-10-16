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
        public int EvaluationPeriodDescriptorId { get; set; }
        [Required]
        public string PerformanceEvaluationTitle { get; set; } = string.Empty;
        [Required]
        public int PerformanceEvaluationTypeDescriptorId { get; set; }
        [Required]
        public short SchoolYear { get; set; }
        [Required]
        public int TermDescriptorId { get; set; }
        public DateTime ActualDate { get; set; }
        public int? ActualDuration { get; set; }
        public int? PerformanceEvaluationRatingLevelDescriptorId { get; set; }
        public TimeSpan? ActualTime { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        [Column("EdFi_Id")]
        [StringLength(50)]
        public string EdFiId { get; set; }
        public string UserId { get; set; } = string.Empty;
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Foreign keys
        [ForeignKey("UserId")]
        public ApplicationUser? ApplicationUser { get; set; }
    }
}

