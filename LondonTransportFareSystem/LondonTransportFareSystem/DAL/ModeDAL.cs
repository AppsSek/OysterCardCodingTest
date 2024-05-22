using LondonTransportFareSystem.Models;
using Microsoft.Data.SqlClient;

namespace LondonTransportFareSystem.DAL
{
    public class ModeDAL
    {
        private readonly SqlConnection _connection;
        public ModeDAL(SqlConnection sqlConnection)
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
    }
}
