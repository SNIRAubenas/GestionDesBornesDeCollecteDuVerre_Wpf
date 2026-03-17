using MySql.Data.MySqlClient;

namespace WpfAppGestionDesBornes.Core
{
    public class DatabaseManager
    {
        // Chaîne de connexion vers la base de données locale
        private string connectionString =
            "Server=localhost;" +
            "Database=gestion_bornes;" +
            "User=root;" +
            "Password=;";

        public MySqlConnection GetConnection()
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            // Ouverture de la connexion
            connection.Open();
            // Retourne une connexion ouverte prête à être utilisée
            return connection;
        }
    }
}