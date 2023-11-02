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
    Task<Status> GetStatusByText(string description);
    Task<List<Evaluation>> GetAllEvaluations();
    Task<PerformanceEvaluation> GetPerformanceEvaluationById(int performanceEvaluationId);
    Task<List<PerformanceEvaluation>> GetAllPerformanceEvaluations();
    Task UpdatePerformanceEvaluations(List<PerformanceEvaluation> performanceEvaluations);
    Task<List<EvaluationObjective>> GetAllEvaluationObjectives();
    Task<EvaluationObjective> GetEvaluationObjectiveById(int id);
    Task<EvaluationObjectiveRating> GetEvaluationObjectiveRatingById(int id);
    Task<List<EvaluationElement>> GetAllEvaluationElements();
    Task<EvaluationElement> GetEvaluationElementById(int id);
    Task<EvaluationElementRatingResult> GetEvaluationElementRatingResultById(int id);
    Task<List<PerformanceEvaluationRating>> GetAllPerformanceEvaluationRatings();
    Task UpdateEvaluationObjectives(List<EvaluationObjective> evaluationObjects);
    Task UpdateEvaluationElements(List<EvaluationElement> evaluationElements);
    Task<List<int>> UpdateEvaluationRatings(List<EvaluationRating> evaluationRatings);
    Task<List<int>> UpdateEvaluationObjectiveRatings(List<EvaluationObjectiveRating> evaluationObjectiveRatings);
    Task<List<int>> UpdateEvaluationElementRatingResults(List<EvaluationElementRatingResult> evaluationElementRatingResults);
    Task<List<EvaluationRating>> GetEvaluationRatingsByUserId(string userId);
    Task<EvaluationRating> GetEvaluationRatingById(int id);
    Task<List<PerformanceEvaluationRating>> GetPerformanceEvaluationRatingsByUserId(string userId);
    Task<PerformanceEvaluationRating> GetPerformanceEvaluationRatingById(int id);
    Task CreatePerformanceEvaluationRating(PerformanceEvaluationRating rating);
    Task<List<int>> UpdatePerformanceEvaluationRatings(List<PerformanceEvaluationRating> performanceEvaluationRatings);
}
