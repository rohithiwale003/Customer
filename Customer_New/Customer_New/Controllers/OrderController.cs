using Customer.Other;
using Customer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Customer.Controllers
{
    [Authorize]
    [Route("api/[Controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        [HttpGet]
        [Route("order-list")]
        public IActionResult GetOrderList()
        {
            OrderServices orderServices = new OrderServices();
            return new ObjectResult(orderServices.GetOrderList());
        }

        //search by order id, customer id, employee id or by ship name
        [HttpGet]
        [Route("order-list/search")]
        public IActionResult GetOrderList(string search_value)
        {
            OrderServices orderServices = new OrderServices();
            return new ObjectResult(orderServices.GetOrderList(search_value));

        }
    }
}
