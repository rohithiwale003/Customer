using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Customer.Other;

namespace Customer.Services
{
    public class UserServices
    {
        public static bool checklogin(string username,string password)
        {
            try
            {
                List<SqlParameter> list = new List<SqlParameter>()
                {
                    new SqlParameter(nameof(username),username),
                    new SqlParameter(nameof(password),password),
                };
                DataTable dataTable = DBManage.GetDataTableFromProcedure(list, "PRC_Check_user", DBManage.ConnectionString);
                if (Convert.ToInt32(dataTable.Rows[0][0])>0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
