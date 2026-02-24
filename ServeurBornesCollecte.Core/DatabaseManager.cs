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