// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

using eppeta.webapi.Identity.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eppeta.webapi.Evaluations.Models
{
    public class EvaluationElementRating
    {
        public string EvaluationElementTitle { get; set; } = string.Empty;
        public int? EvaluationElementRatingLevelDescriptorId { get; set; }
        public decimal Rating { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string? EdFi_Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int StatusId { get; set; } = 1;

        // Foreign keys
        [ForeignKey("UserId")]
        public ApplicationUser? ApplicationUser { get; set; }
        [ForeignKey("StatusId")]
        public Status? RecordStatus { get; set; }
    }
}
