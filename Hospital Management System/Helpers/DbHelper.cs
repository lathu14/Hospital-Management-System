using System.Configuration;
using MySql.Data.MySqlClient;

namespace Hospital_Management_System.Helpers
{
    public static class DbHelper
    {
        private static string _connectionString;

        static DbHelper()
        {
            // Reads the connection string named "HospitalDb" from App.config
            _connectionString = ConfigurationManager.ConnectionStrings["HospitalDb"]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                // Fallback to a default connection string (adjust if needed)
                _connectionString = "server=127.0.0.1;database=hospital_management_system;uid=root;pwd=;";
            }
        }

        public static string ConnectionString => _connectionString;

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}
