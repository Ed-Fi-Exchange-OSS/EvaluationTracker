using EdFi.OdsApi.Sdk.Apis.All;
using eppeta.webapi.Evaluations.Data;
using eppeta.webapi.Evaluations.Models;
using Microsoft.Extensions.Caching.Memory;

namespace eppeta.webapi.Service
{
    class OdsSyncService
    {
        private readonly IODSAPIAuthenticationConfigurationService _service;
        private readonly IEvaluationRepository _evaluationRepository;
        private readonly IMemoryCache _memoryCache;
        private const string dataExpirationKey = "DataExpiration";
        private readonly TimeSpan dataExpirationInterval = TimeSpan.FromDays(1);

        public OdsSyncService(IODSAPIAuthenticationConfigurationService service, IEvaluationRepository evaluationRepository, IMemoryCache memoryCache)
        {
            _service = service;
            _evaluationRepository = evaluationRepository;
            _memoryCache = memoryCache;
        }

        public async Task SyncAsync()
        {
            // Check if already synced dependencies in cache
            if (_memoryCache.Get(dataExpirationKey) == null)
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
                // set next expiration time
                var cachedValue = _memoryCache.GetOrCreate(
                    dataExpirationKey,
                    cacheEntry =>
                    {
                        cacheEntry.AbsoluteExpirationRelativeToNow = dataExpirationInterval;
                        return DateTime.Now;
                    });
            }
        }
    }
}
