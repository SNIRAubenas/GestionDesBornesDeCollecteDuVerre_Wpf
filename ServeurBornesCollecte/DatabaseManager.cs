using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ServeurBornesCollecte
{
    public class DatabaseManager
    {
        private string connectionString =
            "Server=localhost;" +
            "Database=gestion_bornes;" +
            "User=root;" +
            "Password=;";

        public MySqlConnection GetConnection()
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            return connection;
        }
    }
}