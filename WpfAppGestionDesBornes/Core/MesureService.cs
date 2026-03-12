using MySql.Data.MySqlClient;
using System;

namespace WpfAppGestionDesBornes.Core
{
    public class MesureService
    {
        private DatabaseManager dbManager = new DatabaseManager();

        public void SaveMesure(int niveau)
        {
            using (MySqlConnection connection = dbManager.GetConnection())
            {
                string sql =
                    "INSERT INTO mesure (id_conteneur, niveau_remplissage, date_mesure) " +
                    "VALUES (1, @niveau, NOW())";

                MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@niveau", niveau);

                command.ExecuteNonQuery();
            }
        }
    }
}