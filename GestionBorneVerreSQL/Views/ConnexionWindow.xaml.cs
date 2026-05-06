using System.Windows;
using MySql.Data.MySqlClient;

namespace GestionBornesCollecte.Views
{
    public partial class ConnexionWindow : Window
    {
        public string Profil = "";

        private string connexion =
        "Server=localhost;" +
        "Database=gestion_bornes;" +
        "Uid=root;" +
        "Password=;" +
        "Connect Timeout=5;";

        public ConnexionWindow()
        {
            InitializeComponent();
        }

        private void BtnConnexion_Click(object sender, RoutedEventArgs e)
        {
            // on met a jour le titre avec le profil choisi
            txtTitreProfil.Text = "Connexion - " + Profil;

            string login = txtLogin.Text;
            string mdp = txtMotDePasse.Password;

            if (login == "" || mdp == "")
            {
                txtErreur.Text = "Remplis tous les champs.";
                return;
            }

            // on cherche dans la base si le login/mdp existe pour ce profil
            bool connecte = false;

            try
            {
                MySqlConnection conn = new MySqlConnection(connexion);
                conn.Open();

                string requete =
                    "SELECT login FROM utilisateur " +
                    "WHERE login = @login AND mot_de_passe = @mdp AND role = @role;";

                MySqlCommand cmd = new MySqlCommand(requete, conn);
                cmd.Parameters.AddWithValue("@login", login);
                cmd.Parameters.AddWithValue("@mdp", mdp);
                cmd.Parameters.AddWithValue("@role", Profil);

                object resultat = cmd.ExecuteScalar();

                if (resultat != null)
                    connecte = true;

                conn.Close();
            }
            catch
            {
                txtErreur.Text = "Impossible de contacter le serveur.";
                return;
            }

            // si connecte on ouvre la bonne fenetre
            if (connecte)
            {
                if (Profil == "Habitant")
                {
                    HabitantWindow fenetre = new HabitantWindow();
                    fenetre.Show();
                    this.Close();
                }
                else if (Profil == "Eboueur")
                {
                    EboueurWindow fenetre = new EboueurWindow();
                    fenetre.Show();
                    this.Close();
                }
                else if (Profil == "Gestionnaire")
                {
                    GestionnaireWindow fenetre = new GestionnaireWindow();
                    fenetre.Show();
                    this.Close();
                }
            }
            else
            {
                txtErreur.Text = "Identifiant ou mot de passe incorrect.";
            }
        }
    }
}