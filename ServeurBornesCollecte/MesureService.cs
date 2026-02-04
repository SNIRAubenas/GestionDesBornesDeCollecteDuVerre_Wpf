using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System;

namespace ServeurBornesCollecte
{
    public class MesureService
    {
        private DatabaseManager dbManager = new DatabaseManager();

        public void AfficherMesures()
        {
            using (MySqlConnection connection = dbManager.GetConnection())
            {
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
            }
        }
    }
}