// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.Identity.Client;

namespace eppeta.webapi.DTO
{
    public class AvailableEvaluationObjective
    {
        public string Name { get; set; } = string.Empty;
        public int EvaluationObjectiveId { get; set; }
        public List<AvailableEvaluationElement> EvaluationElements { get; set; } = new List<AvailableEvaluationElement>();

        public class AvailableEvaluationElement
        {
            public string Name { get; set; } = string.Empty;
            public int EvaluationElementId { get; set; }
        }
    }
}
