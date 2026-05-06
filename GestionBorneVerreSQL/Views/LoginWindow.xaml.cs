using System.Windows;

namespace GestionBornesCollecte.Views
{
    // premiere fenetre au lancement, l utilisateur choisit son profil
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        // on passe le profil a ConnexionWindow pour qu elle sache quoi verifier en BDD
        private void BtnHabitant_Click(object sender, RoutedEventArgs e)
        {
            ConnexionWindow fenetre = new ConnexionWindow();
            fenetre.Profil = "Habitant";
            fenetre.Show();
            this.Close();
        }

        private void BtnEboueur_Click(object sender, RoutedEventArgs e)
        {
            ConnexionWindow fenetre = new ConnexionWindow();
            fenetre.Profil = "Eboueur";
            fenetre.Show();
            this.Close();
        }

        private void BtnGestionnaire_Click(object sender, RoutedEventArgs e)
        {
            ConnexionWindow fenetre = new ConnexionWindow();
            fenetre.Profil = "Gestionnaire";
            fenetre.Show();
            this.Close();
        }
    }
}