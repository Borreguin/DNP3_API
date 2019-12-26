using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DTO;
using DAL;
using UTILS;
using System.Net.Http;
using System.Net;


namespace DNP3_API.Controllers
{
    [Route("api/device")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        // GET: api/Devices
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Devices/5
        /// <summary>
        /// Get configurations for a DNP3 device by his name
        /// </summary>
        /// <param name="device_name"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.HttpGet("{device_name}", Name = "Get")]
        public API_DEVICE_MODEL Get(string device_name)
        {
            _DNP3_client_DB dnp3_db = new _DNP3_client_DB();
            dnp3_db.read_by_device_name(device_name);
            return new API_DEVICE_MODEL();
        }

        // POST api/<controller>
        /// <summary>
        /// Populate a DNP3 device
        /// </summary>
        /// <remarks>This API will insert a DNP3 device.</remarks>
        /// <param name="id"></param>
        /// <response code="200">The new device was created</response>
        /// <response code="400">The new device was not created</response>
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public HttpResponseMessage Post([System.Web.Http.FromBody]API_DEVICE_MODEL device)
        {
            _DNP3_client_DB dnp3_db = new _DNP3_client_DB();
            device.setDeviceCode = device.getCode(device.device_name);
            ReturnInfo result = dnp3_db.insert(device);
            return result.get_http_response();
        }

        // PUT: api/Devices/5
        [Microsoft.AspNetCore.Mvc.HttpPut("{id}")]
        public void Put(int id, [Microsoft.AspNetCore.Mvc.FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [Microsoft.AspNetCore.Mvc.HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
