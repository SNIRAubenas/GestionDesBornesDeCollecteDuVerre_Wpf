using System;
using System.Collections.Generic;
using System.Windows;
using MySql.Data.MySqlClient;
using GestionBornesCollecte.Models;

namespace GestionBornesCollecte.Services
{
    // gere tous les acces a la BDD pour les bornes
    // separer la logique de l interface permet de modifier la BDD sans toucher aux fenetres
    public class BorneService
    {
        private string connexion =
            "Server=localhost;" +
            "Database=gestion_bornes;" +
            "Uid=root;" +
            "Password=;" +
            "Connect Timeout=5;";

        // capacite max en mm, correspond a la valeur dans la table conteneur de Diana
        private int capaciteMax = 3000;

        public List<Borne> GetBornes()
        {
            List<Borne> bornes = new List<Borne>();

            try
            {
                MySqlConnection conn = new MySqlConnection(connexion);
                conn.Open();

                // jointure entre conteneur et site pour avoir toutes les infos en une requete
                string requete =
                    "SELECT c.id_conteneur, s.nom, s.adresse, s.latitude, s.longitude " +
                    "FROM conteneur c " +
                    "JOIN site s ON c.id_site = s.id_site;";

                MySqlCommand cmd = new MySqlCommand(requete, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Borne b = new Borne();
                    b.IdBorne = reader.GetInt32("id_conteneur");
                    b.Nom = reader.GetString("nom");
                    b.Adresse = reader.GetString("adresse");
                    b.Latitude = reader.GetDouble("latitude");
                    b.Longitude = reader.GetDouble("longitude");
                    bornes.Add(b);
                }

                reader.Close();

                // pour chaque borne on cherche la derniere mesure et on calcule le pourcentage
                for (int i = 0; i < bornes.Count; i++)
                {
                    string req2 =
                        "SELECT niveau_remplissage FROM mesure " +
                        "WHERE id_conteneur = @id " +
                        "ORDER BY date_mesure DESC LIMIT 1;";

                    MySqlCommand cmd2 = new MySqlCommand(req2, conn);
                    cmd2.Parameters.AddWithValue("@id", bornes[i].IdBorne);

                    object resultat = cmd2.ExecuteScalar();

                    if (resultat != null)
                    {
                        // conversion mm -> pourcentage par rapport a la capacite max
                        int distanceMm = int.Parse(resultat.ToString());
                        int pourcentage = (distanceMm * 100) / capaciteMax;

                        if (pourcentage > 100)
                            pourcentage = 100;

                        bornes[i].NiveauRemplissage = pourcentage;
                    }
                    else
                    {
                        bornes[i].NiveauRemplissage = 0;
                    }
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur GetBornes : " + ex.Message);
            }

            return bornes;
        }

        // on insere d abord dans site puis dans conteneur
        // parce que conteneur a une clé etrangere vers site
        public void AjouterBorne(Borne b)
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(connexion);
                conn.Open();

                string req1 =
                    "INSERT INTO site (nom, adresse, latitude, longitude) " +
                    "VALUES (@nom, @adresse, @lat, @lng);";

                MySqlCommand cmd1 = new MySqlCommand(req1, conn);
                cmd1.Parameters.AddWithValue("@nom", b.Nom);
                cmd1.Parameters.AddWithValue("@adresse", b.Adresse);
                cmd1.Parameters.AddWithValue("@lat", b.Latitude);
                cmd1.Parameters.AddWithValue("@lng", b.Longitude);
                cmd1.ExecuteNonQuery();

                // LastInsertedId recupere l id auto-incrementé du site qu on vient de creer
                long idSite = cmd1.LastInsertedId;

                string req2 =
                    "INSERT INTO conteneur (id_site, capacite, etat) " +
                    "VALUES (@idSite, 3000, 'actif');";

                MySqlCommand cmd2 = new MySqlCommand(req2, conn);
                cmd2.Parameters.AddWithValue("@idSite", idSite);
                cmd2.ExecuteNonQuery();

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur AjouterBorne : " + ex.Message);
            }
        }

        // UPDATE avec JOIN pour modifier la table site a partir de l id du conteneur
        public void ModifierBorne(Borne b)
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(connexion);
                conn.Open();

                string requete =
                    "UPDATE site s " +
                    "JOIN conteneur c ON c.id_site = s.id_site " +
                    "SET s.nom = @nom, s.adresse = @adresse " +
                    "WHERE c.id_conteneur = @id;";

                MySqlCommand cmd = new MySqlCommand(requete, conn);
                cmd.Parameters.AddWithValue("@nom", b.Nom);
                cmd.Parameters.AddWithValue("@adresse", b.Adresse);
                cmd.Parameters.AddWithValue("@id", b.IdBorne);
                cmd.ExecuteNonQuery();

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur ModifierBorne : " + ex.Message);
            }
        }

        // ordre important : mesures -> conteneur -> site
        // sinon MySQL refuse a cause des contraintes de clés etrangeres
        public void SupprimerBorne(int idConteneur)
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(connexion);
                conn.Open();

                // on recupere l id du site avant de supprimer le conteneur
                string req0 = "SELECT id_site FROM conteneur WHERE id_conteneur = @id;";
                MySqlCommand cmd0 = new MySqlCommand(req0, conn);
                cmd0.Parameters.AddWithValue("@id", idConteneur);
                object idSiteObj = cmd0.ExecuteScalar();
                long idSite = long.Parse(idSiteObj.ToString()!);

                string req1 = "DELETE FROM mesure WHERE id_conteneur = @id;";
                MySqlCommand cmd1 = new MySqlCommand(req1, conn);
                cmd1.Parameters.AddWithValue("@id", idConteneur);
                cmd1.ExecuteNonQuery();

                string req2 = "DELETE FROM conteneur WHERE id_conteneur = @id;";
                MySqlCommand cmd2 = new MySqlCommand(req2, conn);
                cmd2.Parameters.AddWithValue("@id", idConteneur);
                cmd2.ExecuteNonQuery();

                string req3 = "DELETE FROM site WHERE id_site = @idSite;";
                MySqlCommand cmd3 = new MySqlCommand(req3, conn);
                cmd3.Parameters.AddWithValue("@idSite", idSite);
                cmd3.ExecuteNonQuery();

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur SupprimerBorne : " + ex.Message);
            }
        }

        // on insere une mesure a 0 pour indiquer que la borne a ete vidée
        public void ViderBorne(int idConteneur)
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(connexion);
                conn.Open();

                string requete =
                    "INSERT INTO mesure (id_conteneur, niveau_remplissage, date_mesure) " +
                    "VALUES (@id, 0, NOW());";

                MySqlCommand cmd = new MySqlCommand(requete, conn);
                cmd.Parameters.AddWithValue("@id", idConteneur);
                cmd.ExecuteNonQuery();

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur ViderBorne : " + ex.Message);
            }
        }
    }
}