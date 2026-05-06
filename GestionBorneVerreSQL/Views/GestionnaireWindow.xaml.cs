using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using GestionBornesCollecte.Models;
using GestionBornesCollecte.Services;

namespace GestionBornesCollecte.Views
{
    // vue administrateur, le gestionnaire peut ajouter, modifier et supprimer des bornes
    public partial class GestionnaireWindow : Window
    {
        private List<Borne> bornes = new List<Borne>();

        public GestionnaireWindow()
        {
            InitializeComponent();
            ChargerBornes();
        }

        private void ChargerBornes()
        {
            BorneService service = new BorneService();
            bornes = service.GetBornes();
            lstBornes.ItemsSource = null;
            lstBornes.ItemsSource = bornes;
        }

        // quand on selectionne une borne on remplit automatiquement le formulaire
        private void lstBornes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Borne borne = (Borne)lstBornes.SelectedItem;
            if (borne == null)
                return;

            txtNom.Text = borne.Nom;
            txtAdresse.Text = borne.Adresse;
            txtNiveau.Text = borne.NiveauRemplissage.ToString();
        }

        private void BtnAjouter_Click(object sender, RoutedEventArgs e)
        {
            if (txtNom.Text == "")
            {
                txtMessage.Text = "Le nom est obligatoire.";
                return;
            }

            Borne nouvelle = new Borne();
            nouvelle.Nom = txtNom.Text;
            nouvelle.Adresse = txtAdresse.Text;

            BorneService service = new BorneService();
            service.AjouterBorne(nouvelle);

            txtMessage.Text = "Borne ajoutee.";
            BtnVider_Click(sender, e);
            ChargerBornes();
        }

        private void BtnModifier_Click(object sender, RoutedEventArgs e)
        {
            Borne borne = (Borne)lstBornes.SelectedItem;
            if (borne == null)
            {
                txtMessage.Text = "Selectionne une borne dans la liste.";
                return;
            }

            borne.Nom = txtNom.Text;
            borne.Adresse = txtAdresse.Text;

            BorneService service = new BorneService();
            service.ModifierBorne(borne);

            txtMessage.Text = "Borne modifiee.";
            ChargerBornes();
        }

        private void BtnSupprimer_Click(object sender, RoutedEventArgs e)
        {
            Borne borne = (Borne)lstBornes.SelectedItem;
            if (borne == null)
            {
                txtMessage.Text = "Selectionne une borne dans la liste.";
                return;
            }

            BorneService service = new BorneService();
            service.SupprimerBorne(borne.IdBorne);

            txtMessage.Text = "Borne supprimee.";
            BtnVider_Click(sender, e);
            ChargerBornes();
        }

        private void BtnVider_Click(object sender, RoutedEventArgs e)
        {
            txtNom.Text = "";
            txtAdresse.Text = "";
            txtNiveau.Text = "0";
            lstBornes.SelectedItem = null;
            txtMessage.Text = "";
        }

        private void BtnRetour_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();
        }
    }
}