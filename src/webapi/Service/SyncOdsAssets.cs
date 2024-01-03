using EdFi.OdsApi.Sdk.Apis.All;
using eppeta.webapi.Evaluations.Data;
using eppeta.webapi.Evaluations.Models;

namespace eppeta.webapi.Service
{
    class SyncOdsAssets
    {
        private readonly IODSAPIAuthenticationConfigurationService _service;
        private readonly IEvaluationRepository _evaluationRepository;

        public SyncOdsAssets(IODSAPIAuthenticationConfigurationService service, IEvaluationRepository evaluationRepository)
        {
            _service = service;
            _evaluationRepository = evaluationRepository;
        }

        public async Task SyncAsync()
        {
            // Refresh Evaluation data from API
            // Get ODS/API token
            var authenticatedConfiguration = await _service.GetAuthenticatedConfiguration();

            //// Get Evaluation Objectives and update repository
            var objectivesApi = new EvaluationObjectivesApi(authenticatedConfiguration);
            objectivesApi.Configuration.DefaultHeaders.Add("Content-Type", "application/json");
            var tpdmEvaluationObjectives = await objectivesApi.GetEvaluationObjectivesAsync(limit: 100, offset: 0);
            await _evaluationRepository
                .UpdateEvaluationObjectives(tpdmEvaluationObjectives.Select(teo => (EvaluationObjective)teo).ToList());

            // Get Evaluation Elements which contain the EvaluationObjectiveTitles and update repository
            var elementsApi = new EvaluationElementsApi(authenticatedConfiguration);
            elementsApi.Configuration.DefaultHeaders.Add("Content-Type", "application/json");
            var tpdmEvaluationElements = await elementsApi.GetEvaluationElementsAsync(limit: 100, offset: 0);
            await _evaluationRepository.UpdateEvaluationElements(tpdmEvaluationElements.Select(tee => (EvaluationElement)tee).ToList());

            var peApi = new PerformanceEvaluationsApi(authenticatedConfiguration);
            peApi.Configuration.DefaultHeaders.Add("Content-Type", "application/json");
            var tpdmPerformanceEvaluations = await peApi.GetPerformanceEvaluationsAsync(limit: 100, offset: 0);
            var performanceEvaluations = tpdmPerformanceEvaluations.Select(pe => (PerformanceEvaluation)pe).ToList();
            await _evaluationRepository.UpdatePerformanceEvaluations(performanceEvaluations);

            // Get Candidates
            var candidadesApi = new CandidatesApi(authenticatedConfiguration);
            candidadesApi.Configuration.DefaultHeaders.Add("Content-Type", "application/json");
            var tpdmCandidates = await candidadesApi.GetCandidatesAsync(limit: 100, offset: 0);
            var candidates = tpdmCandidates.Where(c => c.PersonReference != null).Select(c => (Candidate)c).ToList();
            await _evaluationRepository.UpdateCandidates(candidates);
        }
    }
}
