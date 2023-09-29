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
        public int EducationOrganizationId { get; set; }
        public int EvaluationPeriodDescriptorId { get; set; }
        public string PerformanceEvaluationTitle { get; set; } = string.Empty;
        public int PerformanceEvaluationTypeDescriptorId { get; set; }
        public short SchoolYear { get; set; }
        public int TermDescriptorId { get; set; }
        public DateTime ActualDate { get; set; }
        public int? ActualDuration { get; set; }
        public int? PerformanceEvaluationRatingLevelDescriptorId { get; set; }
        public TimeSpan? ActualTime { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string? EdFi_Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Foreign keys
        [ForeignKey("UserId")]
        public ApplicationUser? ApplicationUser { get; set; }
    }
}

