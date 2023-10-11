// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

using eppeta.webapi.Evaluations.Models;
using eppeta.webapi.Identity.Models;
using System.Diagnostics;

namespace eppeta.webapi.Evaluations.Data;

public interface IEvaluationRepository
{
    Task<List<Evaluation>> GetAllEvaluations();
    Task<PerformanceEvaluation> GetPerformanceEvaluationById(int performanceEvaluationId);
    Task<List<PerformanceEvaluation>> GetAllPerformanceEvaluations();
    Task UpdatePerformanceEvaluations(List<PerformanceEvaluation> performanceEvaluations);
    Task<List<EvaluationObjective>> GetAllEvaluationObjectives();
    Task<EvaluationObjective> GetEvaluationObjectiveById(int id);
    Task<List<EvaluationElement>> GetAllEvaluationElements();
    Task<EvaluationElement> GetEvaluationElementById(int id);
    Task<List<PerformanceEvaluationRating>> GetAllPerformanceEvaluationRatings();
    Task UpdateEvaluationObjectives(List<EvaluationObjective> evaluationObjects);
    Task UpdateEvaluationElements(List<EvaluationElement> evaluationElements);
    Task UpdateEvaluationRatings(List<EvaluationRating> evaluationRatings);
    Task UpdateEvaluationObjectiveRatings(List<EvaluationObjectiveRating> evaluationObjectiveRatings);
    Task UpdateEvaluationElementRatingResults(List<EvaluationElementRatingResult> evaluationElementRatingResults);
    Task<List<EvaluationRating>> GetEvaluationRatingsByUserId(string userId);
    Task<List<PerformanceEvaluationRating>> GetPerformanceEvaluationRatingsByUserId(string userId);
    Task<PerformanceEvaluationRating> GetPerformanceEvaluationRatingById(int id);

    Task CreatePerformanceEvaluationRating(PerformanceEvaluationRating rating);

    Task UpdatePerformanceEvaluationRating(PerformanceEvaluationRating rating);
}
