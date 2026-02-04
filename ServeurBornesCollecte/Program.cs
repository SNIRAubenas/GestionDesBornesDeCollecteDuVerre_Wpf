using MySql.Data.MySqlClient;
namespace ServeurBornesCollecte
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString =
            "Server=localhost;" +
            "Database=gestion_bornes;" +
            "User=root;" +
            "Password=;";

            try
            {
                MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                Console.WriteLine("Connexion à la base de données réussie.\n");

                string sql =
                    "SELECT niveau_remplissage, date_mesure " +
                    "FROM mesure " +
                    "ORDER BY date_mesure DESC";

                MySqlCommand command = new MySqlCommand(sql, connection);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int niveau = reader.GetInt32("niveau_remplissage");
                    DateTime date = reader.GetDateTime("date_mesure");

                    Console.WriteLine($"Niveau : {niveau}% | Date : {date}");
                }

                reader.Close();
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
            }
        }
    }
}