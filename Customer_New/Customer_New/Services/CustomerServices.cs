using Customer.Model;
using Customer.Other;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Customer.Services
{
    public class CustomerServices
    {
        private readonly string _connectionString = DBManage.ConnectionString;
        private CustomerModel customerModel;
        private string search_key { get; set; }
        
        public Responce GetCustomerList()
        {
            Responce responce = new Responce();
            try
            {
                DataTable DTCustomerList = DBManage.GetDataTableFromProcedure(null, "PRC_Customer_Search", _connectionString);
                responce.payload = DatatableToModelList(DTCustomerList);
                responce.message = "The data of customer list(all customer)";
                responce.result  = Responce.ActionResult.success.ToString();
            }
            catch(Exception ex)
            {
                responce.message = "something gets wrong....";
                responce.result = Responce.ActionResult.failure.ToString();
            }
            return responce;
        }

        List<CustomerModel> DatatableToModelList(DataTable DTCustomerList)
        {
            return DTCustomerList.AsEnumerable().Select(item => new CustomerModel()
            {
                CustomerID = item[nameof(customerModel.CustomerID)].ToString(),
                CompanyName = item[nameof(customerModel.CompanyName)].ToString(),
                ContactName = item[nameof(customerModel.ContactName)].ToString(),
                ContactTitle = item[nameof(customerModel.ContactTitle)].ToString(),
                Address = item[nameof(customerModel.Address)].ToString(),
                City = item[nameof(customerModel.City)].ToString(),
                Region = item[nameof(customerModel.Region)].ToString(),
                PostalCode = item[nameof(customerModel.PostalCode)].ToString(),
                Country = item[nameof(customerModel.Country)].ToString(),
                Phone = item[nameof(customerModel.Phone)].ToString(),
                Fax = item[nameof(customerModel.Fax)].ToString(),

            }).ToList();
        }

        public Responce GetCustomerList(string searchKey)
        {
            Responce responce = new Responce();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter(nameof(search_key),searchKey)
                };
                DataTable DTCustomerList = DBManage.GetDataTableFromProcedure(sqlParameters, "PRC_Customer_Search", _connectionString);
             
                responce.payload = DatatableToModelList(DTCustomerList);
                responce.message = "The data of customer list";
                responce.result  = Responce.ActionResult.success.ToString();
            }
            catch
            {
                responce.message = "something gets wrong....";
                responce.result  = Responce.ActionResult.failure.ToString();
            }
            return responce;
        }
    }
}
