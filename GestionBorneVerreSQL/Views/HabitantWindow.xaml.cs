using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using GestionBornesCollecte.Models;
using GestionBornesCollecte.Services;

namespace GestionBornesCollecte.Views
{
    // vue habitant, lecture seule avec recherche et systeme de favoris
    public partial class HabitantWindow : Window
    {
        private List<Borne> bornes = new List<Borne>();
        private List<Borne> favoris = new List<Borne>();

        // on sauvegarde les favoris dans un fichier txt, simple et suffisant pour notre besoin
        private string fichierFavoris = "favoris.txt";

        public HabitantWindow()
        {
            InitializeComponent();
            ChargerBornes();
            ChargerFavoris();
        }

        private void ChargerBornes()
        {
            BorneService service = new BorneService();
            bornes = service.GetBornes();
            lstBornes.ItemsSource = null;
            lstBornes.ItemsSource = bornes;
        }

        // lit le fichier et retrouve les objets Borne correspondants dans la liste
        private void ChargerFavoris()
        {
            favoris = new List<Borne>();

            if (!File.Exists(fichierFavoris))
                return;

            string[] lignes = File.ReadAllLines(fichierFavoris);

            for (int i = 0; i < lignes.Length; i++)
            {
                int id = int.Parse(lignes[i]);
                for (int j = 0; j < bornes.Count; j++)
                {
                    if (bornes[j].IdBorne == id)
                    {
                        favoris.Add(bornes[j]);
                        break;
                    }
                }
            }
        }

        private void SauvegarderFavoris()
        {
            List<string> lignes = new List<string>();
            for (int i = 0; i < favoris.Count; i++)
                lignes.Add(favoris[i].IdBorne.ToString());

            File.WriteAllLines(fichierFavoris, lignes);
        }

        private void lstBornes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Borne borne = (Borne)lstBornes.SelectedItem;
            if (borne == null)
                return;

            txtNom.Text = borne.Nom;
            txtAdresse.Text = borne.Adresse;
            txtNiveau.Text = "Remplissage : " + borne.NiveauRemplissage + " %";

            if (borne.NiveauRemplissage >= 85)
                borderAlerte.Visibility = Visibility.Visible;
            else
                borderAlerte.Visibility = Visibility.Collapsed;

            // on met a jour le bouton selon si la borne est deja en favoris ou pas
            bool estFavori = false;
            for (int i = 0; i < favoris.Count; i++)
            {
                if (favoris[i].IdBorne == borne.IdBorne)
                {
                    estFavori = true;
                    break;
                }
            }

            btnFavori.Content = estFavori ? "Retirer des favoris" : "Ajouter aux favoris";
        }

        // filtre en temps reel sur le nom et l adresse, declenché a chaque caractere tapé
        private void txtRecherche_TextChanged(object sender, TextChangedEventArgs e)
        {
            string recherche = txtRecherche.Text.ToLower();
            List<Borne> resultats = new List<Borne>();

            for (int i = 0; i < bornes.Count; i++)
            {
                if (bornes[i].Nom.ToLower().Contains(recherche) ||
                    bornes[i].Adresse.ToLower().Contains(recherche))
                {
                    resultats.Add(bornes[i]);
                }
            }

            lstBornes.ItemsSource = null;
            lstBornes.ItemsSource = resultats;
        }

        private void BtnToutes_Click(object sender, RoutedEventArgs e)
        {
            txtRecherche.Text = "";
            lstBornes.ItemsSource = null;
            lstBornes.ItemsSource = bornes;
        }

        private void BtnFavoris_Click(object sender, RoutedEventArgs e)
        {
            lstBornes.ItemsSource = null;
            lstBornes.ItemsSource = favoris;
        }

        // si la borne est deja en favoris on la retire, sinon on l ajoute
        private void BtnToggleFavori_Click(object sender, RoutedEventArgs e)
        {
            Borne borne = (Borne)lstBornes.SelectedItem;
            if (borne == null)
                return;

            bool estFavori = false;
            int indexTrouve = -1;

            for (int i = 0; i < favoris.Count; i++)
            {
                if (favoris[i].IdBorne == borne.IdBorne)
                {
                    estFavori = true;
                    indexTrouve = i;
                    break;
                }
            }

            if (estFavori)
            {
                favoris.RemoveAt(indexTrouve);
                btnFavori.Content = "Ajouter aux favoris";
            }
            else
            {
                favoris.Add(borne);
                btnFavori.Content = "Retirer des favoris";
            }

            SauvegarderFavoris();
        }

        private void BtnRetour_Click(object sender, RoutedEventArgs e)
        {
            SauvegarderFavoris();
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();
        }
    }
}