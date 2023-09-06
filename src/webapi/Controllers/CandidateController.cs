using EdFi.OdsApi.Sdk.Apis.All;
using eppeta.webapi.DTO;
using eppeta.webapi.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eppeta.webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidateController : ControllerBase
    {
        private readonly IODSAPIAuthenticationConfigurationService _service;
        public CandidateController(IODSAPIAuthenticationConfigurationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<Dictionary<string, List<string>>>> GetCandidates()
        {
            // Get ODS/API token
            var authenticatedConfiguration = _service.GetAuthenticatedConfiguration();

            var candidatesApi = new CandidatesApi(authenticatedConfiguration);
            candidatesApi.Configuration.DefaultHeaders.Add("Content-Type", "application/json");
            //TODO remove limit and filter by current term and school year
            var candidates = await candidatesApi.GetCandidatesAsync(limit: 25, offset: 0);
            var candidatesDictionary = new List<Candidate>();

            candidatesDictionary = candidates.Select(x => new Candidate { CandidateName = $"{x.FirstName} {x.LastSurname}", PersonId = x.PersonReference?.PersonId??"" }).ToList();

            return Ok(candidatesDictionary);
        }
    }
}
