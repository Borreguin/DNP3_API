using System;
using System.Collections.Generic;
using System.Text;

namespace UTILS
{
    public class ReturnInfo
    {
        public bool succesful { get; set; }
        public string message { get; set; }
        public Exception inner_exception { get; set; }
    }
}
