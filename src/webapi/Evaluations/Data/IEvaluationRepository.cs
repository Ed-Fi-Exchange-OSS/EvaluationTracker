// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

using eppeta.webapi.Evaluations.Models;

namespace eppeta.webapi.Evaluations.Data;

public interface IEvaluationRepository
{
    Task<List<Status>> GetAllStatuses();
    Task<Status> GetStatusByText(string description);
    Task<Status> GetStatusById(int id);
    Task<Evaluation> GetEvaluationById(int id);
    Task<List<Evaluation>> GetEvaluationsByPK(object samePKObject);
    Task<List<PerformanceEvaluation>> GetPerformanceEvaluationByPK(object samePKObject);
    Task<List<Evaluation>> GetAllEvaluations();
    Task UpdateEvaluations(List<Evaluation> evaluations);
    Task<PerformanceEvaluation> GetPerformanceEvaluationById(int performanceEvaluationId);
    Task<List<PerformanceEvaluation>> GetPerformanceEvaluationsByPK(object samePKObject);
    Task<List<PerformanceEvaluationRating>> GetPerformanceEvaluationRatingsByPK(object samePKObject);
    Task<List<PerformanceEvaluation>> GetAllPerformanceEvaluations();
    Task UpdatePerformanceEvaluations(List<PerformanceEvaluation> performanceEvaluations);
    Task UpdateCandidates(List<Candidate> candidates);
    Task<List<EvaluationObjective>> GetAllEvaluationObjectives();
    Task<List<Candidate>> GetAllCandidates();
    Task<EvaluationObjective> GetEvaluationObjectiveById(int id);
    Task<List<EvaluationObjective>> GetEvaluationObjectivesByPK(object samePKObject);
    Task<List<EvaluationObjectiveRating>> GetEvaluationObjectiveRatingsByPK(object samePKObject);
    Task<EvaluationObjectiveRating> GetEvaluationObjectiveRatingById(int id);
    Task<List<EvaluationElement>> GetAllEvaluationElements();
    Task<EvaluationElement> GetEvaluationElementById(int id);
    Task<List<EvaluationElement>> GetEvaluationElementsByPK(object samePKObject);
    Task<List<EvaluationElementRatingResult>> GetEvaluationElementRatingResultsByPK(object samePKObject);
    Task<EvaluationElementRatingResult> GetEvaluationElementRatingResultById(int id);
    Task<List<PerformanceEvaluationRating>> GetAllPerformanceEvaluationRatings();
    Task UpdateEvaluationObjectives(List<EvaluationObjective> evaluationObjects);
    Task UpdateEvaluationElements(List<EvaluationElement> evaluationElements);
    Task<List<int>> UpdateEvaluationRatings(List<EvaluationRatingForUpdate> evaluationRatings);
    Task<List<int>> UpdateEvaluationObjectiveRatings(List<EvaluationObjectiveRatingForUpdate> evaluationObjectiveRatings);
    Task<List<int>> UpdateEvaluationElementRatingResults(List<EvaluationElementRatingResultForUpdate> evaluationElementRatingResults);
    Task<List<EvaluationRating>> GetEvaluationRatingsByUserId(string userId);
    Task<List<EvaluationRating>> GetAllEvaluationRatings();
    Task<EvaluationRating> GetEvaluationRatingById(int id);
    Task<List<EvaluationRating>> GetEvaluationRatingsByPK(object samePKObject);
    Task<List<PerformanceEvaluationRating>> GetPerformanceEvaluationRatingsByUserId(string userId);
    Task<PerformanceEvaluationRating> GetPerformanceEvaluationRatingById(int id);
    Task CreatePerformanceEvaluationRating(PerformanceEvaluationRating rating);
    Task<List<int>> UpdatePerformanceEvaluationRatings(List<PerformanceEvaluationRatingForUpdate> performanceEvaluationRatings);
}
