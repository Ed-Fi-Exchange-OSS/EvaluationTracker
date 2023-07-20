//// SPDX-License-Identifier: Apache-2.0
//// Licensed to the Ed-Fi Alliance under one or more agreements.
//// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
//// See the LICENSE and NOTICES files in the project root for more information.

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using EdFi.OdsApi.Sdk.Apis.All;
//using EdFi.OdsApi.Sdk.Models.All;
//using Microsoft.AspNetCore.Mvc;
//using EdFi.OdsApi.Sdk.Client;
//using EdFi.OdsApi.SdkClient;

//namespace webapi.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class EvaluationApiController : ControllerBase
//    {
//        [HttpGet]
//        public static async Task<ActionResult<IEnumerable<TpdmEvaluationObjective>>> GetEvaluation(Configuration configuration)
//        {
//            // GET evaluationObjectivess
//            var apiInstance = new EvaluationObjectivesApi(configuration);
//            apiInstance.Configuration.DefaultHeaders.Add("Content-Type", "application/json");

//            // Fetch a single record with the totalCount flag set to true to retrieve the total number of records available
//            var evaluationObjectivesWithHttpInfo = apiInstance.GetEvaluationObjectivesWithHttpInfo(limit: 25, offset: 0, totalCount: true);

//            var httpReponseCode = evaluationObjectivesWithHttpInfo.StatusCode; // returns System.Net.HttpStatusCode.OK
//            Console.WriteLine("Response code is " + httpReponseCode);

//            // Parse the total count value out of the "Total-Count" response header
//            //int.TryParse(evaluationObjectivesWithHttpInfo.Headers["Total-Count"].First(), out var totalCount);

//            //int offset = 0;
//            //int limit = 100;
//            var evaluationObjectives = new List<TpdmEvaluationObjective>();
//            var evaluationObjectiveTitles = new List<string>();
//            foreach (var evaluationObjective in evaluationObjectives)
//            {
//                evaluationObjectiveTitles.Add(evaluationObjective.EvaluationObjectiveTitle);
//            }

//            //while (offset < totalCount)
//            //{
//            //    Console.WriteLine($"Fetching Evaluation Objectives records {offset} through {Math.Min(offset + limit, totalCount)} of {totalCount}");
//            //    evaluationObjectives.AddRange(apiInstance.GetEvaluationObjectives(limit: limit, offset: 0));
//            //    offset += limit;
//            //}

//            //Console.WriteLine();
//            //Console.WriteLine("Evaluation Objectives Results");

//            //foreach (var evaluationObjective in evaluationObjectives)
//            //{
//            //    Console.WriteLine($"evaluationObjectives: {evaluationObjective.Id}, {evaluationObjective.EvaluationObjectiveTitle}");
//            //}

//            //Console.WriteLine();
//            //Console.WriteLine("Hit ENTER key to continue...");
//            //Console.ReadLine();

//            return await ;
//        }
//    }
//}
