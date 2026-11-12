using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Api_call
{
    /// <summary>
    /// Logique d'interaction pour RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public AuthViewModel ViewModel { get; set; }

        public RegisterWindow()
        {
            InitializeComponent();
            ViewModel = new AuthViewModel();
            DataContext = ViewModel;
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Password = PasswordBox.Password; // Récupérer le mot de passe depuis le PasswordBox
            if (ViewModel.Register())
            {
                LoginWindow login = new LoginWindow();
                login.Show();
                Close(); // Fermer la fenêtre après une inscription réussie
            }
        }
    }
}
