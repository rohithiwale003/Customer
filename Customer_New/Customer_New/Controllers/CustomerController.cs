using Customer.Other;
using Customer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Customer.Controllers
{
    [Authorize]
    [Route("api/[Controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        [HttpGet]
        [Route("customer-list")]
        public IActionResult GetCustomerList()
        {
            CustomerServices customerServices = new CustomerServices();
            return new ObjectResult(customerServices.GetCustomerList());
        }

        //search by company name, contact name or address 
        [HttpGet]
        [Route("customer-list/search")]
        public IActionResult GetCustomerList(string search_value)
        {
            CustomerServices customerServices = new CustomerServices();
            return new ObjectResult(customerServices.GetCustomerList(search_value));
        }
    }
}
