using System.Windows;
using GestionBornesCollecte.Views;

namespace GestionBornesCollecte.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Hide();
        }
    }
}