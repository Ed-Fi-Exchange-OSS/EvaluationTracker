// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace eppeta.webapi.DTO
{
    public class PerformedEvaluation
    {
        public string PerformanceEvaluationTitle { get; set; } = string.Empty;
        public string EvaluationTitle { get; set; } = string.Empty;
        public DateTime ActualDate { get; set; }
        public string? EvaluatorName { get; set; } = string.Empty;
        public string EvaluationStatus { get; set; } = string.Empty;
        public string? ReviewedCandidateName { get; set; } = string.Empty;
        public string ReviewedPersonId { get; set; } = string.Empty;
        public string ReviewedPersonIdSourceSystemDescriptor { get; set; } = string.Empty;
        public int PerformanceEvaluationRatingId { get; set; }
    }
}
