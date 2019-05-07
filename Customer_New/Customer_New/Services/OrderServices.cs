using Customer.Model;
using Customer.Other;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using static Customer.Model.OrderModel;

namespace Customer.Services
{
    public class OrderServices
    {
        private readonly string _connectionString = DBManage.ConnectionString;
        private OrderModel orderModel;
        private OrderDetailsModel orderDetailsModel;
        private string search_key { get; set; }

        public Responce GetOrderList()
        {
            Responce responce = new Responce();
            try
            {
                DataSet DSOrderList = DBManage.GetDataSetFromProcedure(null, "PRC_OrderList_Search", _connectionString);
                responce.payload = DataSetToModelList(DSOrderList);
                responce.message = "The data of order list(all orders)";
                responce.result = Responce.ActionResult.success.ToString();
            }
            catch(Exception ex)
            {
                responce.message = "something gets wrong....";
                responce.result = Responce.ActionResult.failure.ToString();
            }
            return responce;
        }

        List<OrderModel> DataSetToModelList(DataSet DSOrderList)
        {
            return DSOrderList.Tables[0].AsEnumerable().Select(item => new OrderModel()
            {
                OrderID         = Convert.ToInt32(item[nameof(orderModel.OrderID)]),
                CustomerID      = item[nameof(orderModel.CustomerID    )].ToString(),
                EmployeeID      = Convert.ToInt32(item[nameof(orderModel.EmployeeID)]),
                OrderDate       = item[nameof(orderModel.OrderDate     )].ToString(),
                RequiredDate    = item[nameof(orderModel.RequiredDate  )].ToString(),
                ShippedDate     = item[nameof(orderModel.ShippedDate   )].ToString(),
                ShipVia         = Convert.ToInt32(item[nameof(orderModel.ShipVia)]),
                Freight         = Convert.ToDouble(item[nameof(orderModel.Freight)]),
                ShipName        = item[nameof(orderModel.ShipName      )].ToString(),
                ShipAddress     = item[nameof(orderModel.ShipAddress   )].ToString(),
                ShipCity        = item[nameof(orderModel.ShipCity      )].ToString(),
                ShipRegion      = item[nameof(orderModel.ShipRegion    )].ToString(),
                ShipPostalCode  = item[nameof(orderModel.ShipPostalCode)].ToString(),
                ShipCountry     = item[nameof(orderModel.ShipCountry   )].ToString(),
                OrderDetails    = DataTableTiModelList(Convert.ToInt32(item[nameof(orderModel.OrderID)]),DSOrderList.Tables[1])
            }).ToList();
        }

        List<OrderDetailsModel> DataTableTiModelList(int SearchID,DataTable DTOrderDetails)
        {
            return DTOrderDetails.AsEnumerable().Where(row => Convert.ToInt32(row[nameof(orderModel.OrderID)]) == SearchID).
                    Select(item => new OrderDetailsModel()
                    {
                        OrderID   = Convert.ToInt32(item[nameof(orderDetailsModel.OrderID)]),
                        ProductID = Convert.ToInt32(item[nameof(orderDetailsModel.ProductID)]),
                        UnitPrice = Convert.ToDouble(item[nameof(orderDetailsModel.UnitPrice)]),
                        Quantity  = Convert.ToInt32(item[nameof(orderDetailsModel.Quantity)]),
                        Discount  = Convert.ToDouble(item[nameof(orderDetailsModel.Discount)]),
                    }).ToList();
        }

        public Responce GetOrderList(string searchKey)
        {
            Responce responce = new Responce();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter(nameof(search_key),searchKey)
                };
                DataSet DSOrderList = DBManage.GetDataSetFromProcedure(sqlParameters, "PRC_OrderList_Search", _connectionString);

                responce.payload = DataSetToModelList(DSOrderList);
                responce.message = "The data of order list";
                responce.result = Responce.ActionResult.success.ToString();
            }
            catch
            {
                responce.message = "something gets wrong....";
                responce.result = Responce.ActionResult.failure.ToString();
            }
            return responce;
        }
    }
}