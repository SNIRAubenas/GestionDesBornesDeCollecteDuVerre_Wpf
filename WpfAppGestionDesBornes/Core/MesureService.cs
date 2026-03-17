using MySql.Data.MySqlClient;
using System;

namespace WpfAppGestionDesBornes.Core
{
    public class MesureService
    {
        private DatabaseManager dbManager = new DatabaseManager();

        // Méthode pour sauvegarder une mesure de niveau (distance)
        public void SaveMesure(int niveau)
        {
            // using permet de fermer automatiquement la connexion après utilisation
            using (MySqlConnection connection = dbManager.GetConnection())
            {
                string sql =
                    "INSERT INTO mesure (id_conteneur, niveau_remplissage, date_mesure) " +
                    "VALUES (1, @niveau, NOW())";

                MySqlCommand command = new MySqlCommand(sql, connection);
                // Paramètre sécurisé pour éviter l'injection SQL
                command.Parameters.AddWithValue("@niveau", niveau);

                // Exécution de la requête (INSERT)
                command.ExecuteNonQuery();
            }
        }
    }
}