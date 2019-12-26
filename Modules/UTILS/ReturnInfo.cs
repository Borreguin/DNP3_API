using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace UTILS
{
    public class ReturnInfo
    {
        public bool succesful { get; set; }
        public string message { get; set; }
        public Exception inner_exception { get; set; }

        public HttpResponseMessage get_http_response() {
            HttpResponseMessage http_message = new HttpResponseMessage();
            // HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(this), Encoding.UTF8);
            // http_message.Content = httpContent;
            
            if (succesful)
            {
                http_message.StatusCode = HttpStatusCode.OK;
                http_message.Content = new StringContent(message);
                return http_message;
            }
            else
            {
                http_message.StatusCode = HttpStatusCode.BadRequest;
                http_message.ReasonPhrase = message;
                return http_message;
            }
        }
    }
}
