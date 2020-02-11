using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DNP3_API.Controllers
{
    [Route("api/scan_device")]
    [ApiController]
    public class ScanController : ControllerBase
    {
        // GET: api/Scan
        [HttpGet("new")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        
    }
}
