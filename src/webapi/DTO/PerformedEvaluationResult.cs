// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

namespace eppeta.webapi.DTO
{
    public class PerformedEvaluationResult
    {
        public string EvaluatorName { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string ReviewedPersonId { get; set; } = string.Empty;
        public string ReviewedPersonSourceSystemDescriptor { get; set; } = string.Empty;
        public string? Comments { get; set; } = string.Empty;
        public string ReviewedCandidateName { get; set; } = string.Empty;
        public int EvaluationRatingId { get; set; }
        public int EvaluationId { get; set; }
        public string PerformanceEvaluationTitle { get; set; } = string.Empty;
        public int? StatusId { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public List<PerformedEvaluationResultObjective> ObjectiveResults { get; set; } = new List<PerformedEvaluationResultObjective>();
        public class PerformedEvaluationResultObjective
        {
            public int Id { get; set; }
            public string Comment { get; set; } = string.Empty;
            public List<PerformedEvaluationResultElement> Elements { get; set; } = new List<PerformedEvaluationResultElement>();
        }
        public class PerformedEvaluationResultElement
        {
            public int Id { get; set; }
            public int Score { get; set; }
        }
    }
}
