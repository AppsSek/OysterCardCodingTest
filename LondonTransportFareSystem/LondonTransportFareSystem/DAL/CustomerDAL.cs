using LondonTransportFareSystem.Models;
using Microsoft.Data.SqlClient;

namespace LondonTransportFareSystem.DAL
{
    public class CustomerDAL
    {
        private readonly SqlConnection _connection;
        public CustomerDAL(SqlConnection sqlConnection)
        {
            if (sqlConnection == null)
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

        public Boolean UpdateCustomer(Customer customer)
        {
            _connection.Open();

            return true;
        }

        public void InsertCustomer(Customer customer)
        {
            _connection.Open();
        }

        public void DeleteCustomer(Customer customer)
        {
            _connection.Open();
        }

        public Customer GetCustomerByID(Guid customerId)
        {
            return new Customer();
        }
    }
}
