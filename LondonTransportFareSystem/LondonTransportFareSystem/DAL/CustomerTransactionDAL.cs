using log4net;
using LondonTransportFareSystem.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Transactions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace LondonTransportFareSystem.DAL
{
    public class CustomerTransactionDAL
    {
        private readonly SqlConnection _connection;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public CustomerTransactionDAL(SqlConnection sqlConnection)
        {
            if (sqlConnection == null || sqlConnection.ConnectionString == "")
            {
                _connection = new SqlConnection();
                string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new String[] { @"bin\" }, StringSplitOptions.None)[0];
                IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(projectPath).AddJsonFile("appsettings.json").Build();
                _connection.ConnectionString = configuration.GetConnectionString("SqlConnection");
            }
            else
            {
                _connection = sqlConnection;
            }
        }

        public decimal GetCurrentBalance(Guid customerId)
        {
            decimal balance = 0;
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetCurrentBalance", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@customerID", customerId);
                    var returnParameter = cmd.Parameters.Add("@balance", SqlDbType.Money);
                    returnParameter.Direction = ParameterDirection.Output;
                    _connection.Open();
                    cmd.ExecuteNonQuery();
                    balance = returnParameter.Value != null ? (decimal)returnParameter.Value : 0;
                    
                }
            }
            //All Possible Exceptions like INVALID CAST EXCEPTION goes here - with LOGGING - To NOT lose the STACK Trace
            catch (Exception ex)
            {
                log.Error("Exception occured while showing balance for customer id:" + customerId);
                log.Error(ex);
                throw ex;
            }
            return balance;
        }

        public decimal TopUpAccount(Guid customerId, decimal amount)
        {
            decimal balance = 0;
            try
            {
                using (SqlCommand cmd = new SqlCommand("TopUpAccount", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@customerID", customerId);
                    cmd.Parameters.AddWithValue("@amount", amount);
                    var returnParameter1 = cmd.Parameters.Add("@success", SqlDbType.Bit);
                    returnParameter1.Direction = ParameterDirection.Output;
                    var returnParameter2 = cmd.Parameters.Add("@balance", SqlDbType.Money);
                    returnParameter2.Direction = ParameterDirection.Output;
                    _connection.Open();
                    cmd.ExecuteNonQuery();
                    balance = (decimal)returnParameter2.Value;

                }
            }
            //All Possible Exceptions like INVALID CAST EXCEPTION goes here - with LOGGING - To NOT lose the STACK Trace
            catch (Exception ex)
            {
                log.Error("Exception occured while showing balance for customer id:" + customerId);
                log.Error(ex);
                throw ex;
            }
            return balance;
        }

        public bool SwipeIn(Guid customerId, int mode, int? stationId)
        {
            bool isAllowed = false;
            try
            {
                using (SqlCommand cmd = new SqlCommand("UpdateTransactionOnSwipeIn", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@customerID", customerId);
                    cmd.Parameters.AddWithValue("@modeID", mode);
                    cmd.Parameters.AddWithValue("@stationID", stationId);
                    var returnParameter1 = cmd.Parameters.Add("@isAllowed", SqlDbType.Bit);
                    returnParameter1.Direction = ParameterDirection.Output;
                    _connection.Open();
                    cmd.ExecuteNonQuery();
                    isAllowed = Convert.ToBoolean(returnParameter1.Value);
                }
            }
            //All Possible Exceptions like INVALID CAST EXCEPTION goes here - with LOGGING - To NOT lose the STACK Trace
            catch (Exception ex)
            {
                log.Error("Exception occured while showing balance for customer id:" + customerId);
                log.Error(ex);
                throw ex;
            }
            return isAllowed;
        }

        public bool SwipeOut(Guid customerId, int mode, int? stationId)
        {
            bool isSuccess = false;
            try
            {
                using (SqlCommand cmd = new SqlCommand("UpdateTransactionOnSwipeOut", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@customerID", customerId);
                    cmd.Parameters.AddWithValue("@modeID", mode);
                    cmd.Parameters.AddWithValue("@stationID", stationId);
                    var returnParameter1 = cmd.Parameters.Add("@success", SqlDbType.Bit);
                    returnParameter1.Direction = ParameterDirection.Output;
                    _connection.Open();
                    cmd.ExecuteNonQuery();
                    isSuccess = Convert.ToBoolean(returnParameter1.Value);
                }
            }
            //All Possible Exceptions like INVALID CAST EXCEPTION goes here - with LOGGING - To NOT lose the STACK Trace
            catch (Exception ex)
            {
                log.Error("Exception occured while showing balance for customer id:" + customerId);
                log.Error(ex);
                throw ex;
            }
            return isSuccess;
        }

        public void UpdateTransaction(CustomerTransaction transaction)
        {
            _connection.Open();
        }

        public void InsertTransaction(CustomerTransaction transaction)
        {
            _connection.Open();
        }

        public void DeleteTransaction(CustomerTransaction transaction)
        {
            _connection.Open();
        }

        public CustomerTransaction GetLatestTransaction(Guid customerId)
        {

            return new CustomerTransaction();
        }

        
    }
}
