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
using System.Text.RegularExpressions;


namespace DNP3_API.Controllers
{
    [Route("api/device")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        // GET: api/device
        /// <summary>
        /// Return names of existing devices as a list 
        /// </summary>
        /// <remarks>This return a list of names</remarks>
        /// <response code="204">There is not devices </response>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IEnumerable<string> Get()
        {
            _DNP3_client_DB dnp3_db = new _DNP3_client_DB();
            IEnumerable<API_DEVICE_MODEL> devices = dnp3_db.read_all();
            List<string> d_names = new List<string>();
            foreach (API_DEVICE_MODEL device in devices) {
                d_names.Add(device.device_name);
            }
            return d_names;
        }

        // GET: api/device/<device_name>
        /// <summary>
        /// Get configurations for a DNP3 device by his name
        /// </summary>
        /// <response code="204">There is not device that corresponds to [device_name] </response>
        /// <param name="device_name"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.HttpGet("{device_name}", Name = "Get")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public API_DEVICE_MODEL Get(string device_name)
        {
            _DNP3_client_DB dnp3_db = new _DNP3_client_DB();
            return dnp3_db.read_by_device_name(device_name);
        }

        // POST api/<controller>
        /// <summary>
        /// Populate a DNP3 device
        /// </summary>
        /// <remarks>This API will insert a DNP3 device.</remarks>
        /// <param name="device"> A DNP3 device model</param>
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

        /// <summary>
        /// Updates a DNP3 device if it exists otherwise insert a new one 
        /// </summary>
        /// <param name="device_name"></param>
        /// <param name="device_config"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.HttpPut("{device_name}")]
        public HttpResponseMessage Put(string device_name, [Microsoft.AspNetCore.Mvc.FromBody] API_DEVICE_MODEL device_config)
        {
            _DNP3_client_DB dnp3_db = new _DNP3_client_DB();
            ReturnInfo result = dnp3_db.update(device_name, device_config);
            return new ReturnInfo()
            {
                succesful = result.succesful,
                message = result.succesful? $"Device [{device_name}] was updated." : $"Device [{device_name}] was not updated.",
                inner_exception =  result.succesful? null: result.inner_exception
            }.get_http_response();
        }

        // DELETE: api/ApiWithActions/5
        /// <summary>
        /// Removes a DNP3 device
        /// </summary>
        /// <param name="device_name"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.HttpDelete("{device_name}")]
        public HttpResponseMessage Delete(string device_name)
        {
            _DNP3_client_DB dnp3_db = new _DNP3_client_DB();
            ReturnInfo result = dnp3_db.delete(device_name);
            return result.get_http_response();
        }
    }
}
