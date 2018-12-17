using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Walletemp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class AttendanceController : Controller
    {
        [HttpGet]
        [Route("Test")]
        public async Task<IActionResult> Test()
        {
            return Ok("Endpoint works");
        }
    }
}
