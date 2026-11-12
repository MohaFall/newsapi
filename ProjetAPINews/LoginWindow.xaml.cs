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
    /// Logique d'interaction pour LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public AuthViewModel ViewModel { get; set; }

        public LoginWindow()
        {
            InitializeComponent();
            ViewModel = new AuthViewModel();
            DataContext = ViewModel;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Password = PasswordBox.Password; // Récupérer le mot de passe depuis le PasswordBox
            int userId = ViewModel.Login();

            if (userId > 0)
            {
                // Ouvrir la fenêtre principale avec l'ID de l'utilisateur
                var mainWindow = new MainWindow(userId);
                mainWindow.Show();
                Close(); // Fermer la fenêtre de connexion
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // Ouvrir la fenêtre principale avec l'ID de l'utilisateur
            var registerWindow = new RegisterWindow();
            registerWindow.Show();
            Close(); // Fermer la fenêtre de connexion
        }

    }
}
