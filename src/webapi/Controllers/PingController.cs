// SPDX-License-Identifier: Apache-2.0
// Licensed to the Ed-Fi Alliance under one or more agreements.
// The Ed-Fi Alliance licenses this file to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICES files in the project root for more information.

using Microsoft.AspNetCore.Mvc;

namespace eppeta.webapi.Controllers
{
    // This route provides a method for testing that the service is running.
    [Route("api/[controller]")]
    [ApiController]
    public class PingController : ControllerBase
    {
        [HttpGet]
        public  IActionResult Get()
        {
            return Ok(new { dateTime = DateTime.UtcNow});
        }
    }
}
