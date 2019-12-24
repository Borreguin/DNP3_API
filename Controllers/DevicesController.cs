using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DTO;
using DAL;

namespace DNP3_API.Controllers
{
    [Route("api/device")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        // GET: api/Devices
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Devices/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        /// <summary>
        /// Populate a DNP3 device
        /// </summary>
        /// <remarks>This API will insert a DNP3 device.</remarks>
        /// <param name="id"></param>
        /// <response code="200">The new device was created</response>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public void Post([FromBody]API_DEVICE_MODEL device)
        {
            _DNP3_client_DB dnp3_client = new _DNP3_client_DB();
            dnp3_client.insert(device);
        }

        // PUT: api/Devices/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
