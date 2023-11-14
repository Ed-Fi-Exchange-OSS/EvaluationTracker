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
    public class PerformanceEvaluationRatingLevel
    {
        public int PerformanceEvaluationId { get; set; }
        [Required]
        public string EvaluationRatingLevelDescriptor { get; set; } = string.Empty;
        public decimal? MaxRating { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        // Foreign keys
        [ForeignKey("PerformanceEvaluationId")]
        public PerformanceEvaluation? PerformanceEvaluation { get; set; } = null!;
        public static explicit operator PerformanceEvaluationRatingLevel(TpdmPerformanceEvaluationRatingLevel tpdmPerformanceEvaluationRatingLevel)
            => new PerformanceEvaluationRatingLevel
            {
                EvaluationRatingLevelDescriptor = tpdmPerformanceEvaluationRatingLevel.EvaluationRatingLevelDescriptor,
                MaxRating = (decimal?)tpdmPerformanceEvaluationRatingLevel.MaxRating
            };
    }
}

