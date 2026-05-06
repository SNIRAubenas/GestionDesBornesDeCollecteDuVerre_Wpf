using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using GestionBornesCollecte.Models;
using GestionBornesCollecte.Services;

namespace GestionBornesCollecte.Views
{
    // vue pour les agents de collecte, ils peuvent voir les bornes et les marquer comme vidées
    public partial class EboueurWindow : Window
    {
        private List<Borne> bornes = new List<Borne>();

        public EboueurWindow()
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

        private void lstBornes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Borne borne = (Borne)lstBornes.SelectedItem;
            if (borne == null)
                return;

            txtNom.Text = borne.Nom;
            txtAdresse.Text = borne.Adresse;
            txtNiveau.Text = "Remplissage : " + borne.NiveauRemplissage + " %";

            // on affiche l alerte rouge si la borne est prioritaire
            if (borne.NiveauRemplissage >= 85)
                txtAlerte.Visibility = Visibility.Visible;
            else
                txtAlerte.Visibility = Visibility.Collapsed;
        }

        // trie les bornes par niveau decroissant et garde les 10 premières avec LINQ
        private void BtnTop10_Click(object sender, RoutedEventArgs e)
        {
            List<Borne> top10 = bornes
                .OrderByDescending(b => b.NiveauRemplissage)
                .Take(10)
                .ToList();

            lstBornes.ItemsSource = null;
            lstBornes.ItemsSource = top10;
        }

        private void BtnToutes_Click(object sender, RoutedEventArgs e)
        {
            lstBornes.ItemsSource = null;
            lstBornes.ItemsSource = bornes;
        }

        private void BtnMarquerVidee_Click(object sender, RoutedEventArgs e)
        {
            Borne borne = (Borne)lstBornes.SelectedItem;
            if (borne == null)
            {
                MessageBox.Show("Selectionne une borne dans la liste.");
                return;
            }

            BorneService service = new BorneService();
            service.ViderBorne(borne.IdBorne);

            // on met a jour localement sans recharger toute la BDD
            borne.NiveauRemplissage = 0;
            lstBornes.ItemsSource = null;
            lstBornes.ItemsSource = bornes;
            txtNiveau.Text = "Remplissage : 0 %";
            txtAlerte.Visibility = Visibility.Collapsed;
        }

        private void BtnRetour_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();
        }
    }
}