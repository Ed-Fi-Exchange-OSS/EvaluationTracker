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
using EdFi.OdsApi.Sdk.Models.All;
using Microsoft.EntityFrameworkCore;

namespace eppeta.webapi.Evaluations.Models;

[Table("Evaluation", Schema = "eppeta")]
public partial class Evaluation
{
    [Required]
    public long EducationOrganizationId { get; set; }
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
    public short SchoolYear { get; set; }
    [Required]
    public string TermDescriptor { get; set; }
    public DateTime? CreateDate { get; set; }

    public DateTime? LastModifiedDate { get; set; }
    [Column("EdFi_Id")]
    [StringLength(50)]
    public string? EdFiId { get; set; }
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }


    public static explicit operator TpdmEvaluation(Evaluation evaluation)
        => new TpdmEvaluation
        (
            performanceEvaluationReference: new TpdmPerformanceEvaluationReference
            (
                educationOrganizationId: (int)evaluation.EducationOrganizationId,
                evaluationPeriodDescriptor: evaluation.EvaluationPeriodDescriptor,
                performanceEvaluationTitle: evaluation.PerformanceEvaluationTitle,
                performanceEvaluationTypeDescriptor: evaluation.PerformanceEvaluationTypeDescriptor,
                schoolYear: evaluation.SchoolYear,
                termDescriptor: evaluation.TermDescriptor
            ),
            evaluationTitle: evaluation.EvaluationTitle
        );

    public static explicit operator Evaluation(TpdmEvaluation tpdmEvaluation)
        => new Evaluation
        {
            EducationOrganizationId = tpdmEvaluation.PerformanceEvaluationReference.EducationOrganizationId,
            EvaluationTitle = tpdmEvaluation.EvaluationTitle,
            EvaluationPeriodDescriptor = tpdmEvaluation.PerformanceEvaluationReference.EvaluationPeriodDescriptor,
            PerformanceEvaluationTitle = tpdmEvaluation.PerformanceEvaluationReference.PerformanceEvaluationTitle,
            PerformanceEvaluationTypeDescriptor = tpdmEvaluation.PerformanceEvaluationReference.PerformanceEvaluationTypeDescriptor,
            SchoolYear = (short)tpdmEvaluation.PerformanceEvaluationReference.SchoolYear,
            TermDescriptor = tpdmEvaluation.PerformanceEvaluationReference.TermDescriptor,
            EdFiId = tpdmEvaluation.Id
        };
}
