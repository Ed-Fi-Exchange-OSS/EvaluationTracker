using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eppeta.webapi.Evaluations.Data;
using eppeta.webapi.Evaluations.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace eppeta.webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvaluationRatingController : ControllerBase
    {
        private readonly IEvaluationRepository _evaluationRepository;

        public EvaluationRatingController(IEvaluationRepository evaluationRepository)
        {
            _evaluationRepository = evaluationRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PerformanceEvaluationRating>>> GetPerformanceEvaluationRatings()
        {
            var ratings = await _evaluationRepository.GetAllPerformanceEvaluationRatings();

            return Ok(ratings);
        }
















        //// GET: api/<EvaluationRatingController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<EvaluationRatingController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<EvaluationRatingController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<EvaluationRatingController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<EvaluationRatingController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
