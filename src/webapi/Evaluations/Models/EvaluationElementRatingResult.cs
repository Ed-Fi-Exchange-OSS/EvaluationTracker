// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace eppeta.webapi.Evaluations.Models;

[Table("EvaluationElementRatingResult", Schema = "eppeta")]
public partial class EvaluationElementRatingResult
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
    [Column(TypeName = "decimal(6, 3)")]
    public decimal Rating { get; set; }
    [Required]
    [StringLength(50)]
    public string RatingResultTitle { get; set; }
    public string ResultDatatypeTypeDescriptor { get; set; }
    public DateTime CreateDate { get; set; }
    [Column("EdFi_Id")]
    [StringLength(50)]
    public string EdFiId { get; set; }
    [Required]
    [StringLength(225)]
    public string UserId { get; set; }
    public int StatusId { get; set; } = 1;
    [Key]
    public int Id { get; set; }
    [ForeignKey("StatusId")]
    public Status? RecordStatus { get; set; }
}
