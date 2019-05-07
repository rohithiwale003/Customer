using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Customer.Other
{
    public class Responce
    {
        public string result { get; set; }
        public string message { get; set; }
        public object payload { get; set; }
        public enum ActionResult
        {
            success,
            failure
        }
    }
}
