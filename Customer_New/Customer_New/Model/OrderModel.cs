using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Customer.Model
{
    public class OrderModel
    {
        public int    OrderID        { get; set; }
        public string CustomerID     { get; set; }
        public int    EmployeeID     { get; set; }
        public string OrderDate      { get; set; }
        public string RequiredDate   { get; set; }
        public string ShippedDate    { get; set; }
        public int    ShipVia        { get; set; }
        public double Freight        { get; set; }
        public string ShipName       { get; set; }
        public string ShipAddress    { get; set; }
        public string ShipCity       { get; set; }
        public string ShipRegion     { get; set; }
        public string ShipPostalCode { get; set; }
        public string ShipCountry    { get; set; }
        public List<OrderDetailsModel> OrderDetails { get; set; }

        public class OrderDetailsModel
        {
            public int    OrderID   { get; set; }
            public int    ProductID { get; set; }
            public double UnitPrice { get; set; }
            public int    Quantity  { get; set; }
            public double Discount  { get; set; }

        }

    }
}
