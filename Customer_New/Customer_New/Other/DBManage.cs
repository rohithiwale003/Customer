using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Customer.Other
{
    public class DBManage
    {
        public static string ConnectionString { get; set; } = "Data Source=DESKTOP-AANJQ9K; Initial Catalog=Northwind; User ID=sa; Password=root";
        public static int SetDataFromProcedure(List<SqlParameter> list, string StoreProcedure, string connectionString)
        {
            int ID = 0;
            SqlTransaction sqlTrans = default(SqlTransaction);
            SqlConnection sqlCon = new SqlConnection(connectionString);
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandText = StoreProcedure;
            sqlCmd.Connection = sqlCon;
            sqlCmd.CommandType = CommandType.StoredProcedure;
            if (list != null)
            {
                    sqlCmd.Parameters.AddRange(list.ToArray());
            }
            try
            {
                sqlCon.Open();
                sqlTrans = sqlCon.BeginTransaction();
                sqlCmd.Transaction = sqlTrans;
                ID = Convert.ToInt32(sqlCmd.ExecuteScalar());
                //Commit the transaction in case the stored procedure is executed without errors
                sqlTrans.Commit();
                return ID;
            }
            catch
            {
                //Rollback the transaction in case an exception is raised during the execution of the stored procedure
                throw;
            }
            finally
            {
                //explicitly close the connection
                if (!(sqlCon.State == ConnectionState.Closed))
                {
                    //if the connection is not closed... force to close it.
                    sqlCon.Close();
                }
            }
        }


        public static DataTable GetDataTableFromProcedure(List<SqlParameter> list, string StoreProcedure, string connectionString)
        {
            DataTable dt = new DataTable();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandText = StoreProcedure;
            sqlCmd.Connection = sqlCon;
            sqlCmd.CommandType = CommandType.StoredProcedure;
            if (list != null)
            {
                sqlCmd.Parameters.AddRange(list.ToArray());
            }
            try
            {
                SqlDataAdapter sqlDA = new SqlDataAdapter(sqlCmd);
                //Calling the fill method of the adapter object to fill the data into the dataset - datatable
                sqlDA.Fill(dt);
            }
            catch
            {
                //Rollback the transaction in case an exception is raised during the execution of the stored procedure
                throw;
            }
            finally
            {
                //explicitly close the connection
                if (!(sqlCon.State == ConnectionState.Closed))
                {
                    //if the connection is not closed... force to close it.
                    sqlCon.Close();
                }
            }
            return dt;
        }

        public static DataSet GetDataSetFromProcedure(List<SqlParameter> list, string StoreProcedure, string connectionString)
        {
            DataSet ds = new DataSet();
            SqlConnection sqlCon = new SqlConnection(connectionString);
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandText = StoreProcedure;
            sqlCmd.Connection = sqlCon;
            sqlCmd.CommandType = CommandType.StoredProcedure;
            if (list != null)
            {
                sqlCmd.Parameters.AddRange(list.ToArray());
            }
            try
            {
                SqlDataAdapter sqlDA = new SqlDataAdapter(sqlCmd);
                //Calling the fill method of the adapter object to fill the data into the dataset - datatable
                sqlDA.Fill(ds);
            }
            catch
            {
                //Rollback the transaction in case an exception is raised during the execution of the stored procedure
                throw;
            }
            finally
            {
                //explicitly close the connection
                if (!(sqlCon.State == ConnectionState.Closed))
                {
                    //if the connection is not closed... force to close it.
                    sqlCon.Close();
                }
            }
            return ds;
        }
    }
}
