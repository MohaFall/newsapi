using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NewsAPI;
using NewsAPI.Constants;
using NewsAPI.Models;
using Newtonsoft.Json;

namespace Api_call
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public NewsViewModel ViewModel { get; set; }
        private int _userId;
        public MainWindow(int userId)
        {
            InitializeComponent();
            ViewModel = new NewsViewModel();
            DataContext = ViewModel;
            _userId = userId; // Stocker l'ID de l'utilisateur
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string error = await ViewModel.LoadNewsAsync(ViewModel.SearchTerm);
            if (error != null)
            {
                MessageBox.Show(error, "NewsAPI", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ShowDetails_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is NewsArticle article)
            {
                
                var detailsWindow = new DetailsWindow(article, ViewModel, this._userId);
                detailsWindow.ShowDialog();
            }
        }

        private void ShowFavorites_Click(object sender, RoutedEventArgs e)
        {
            var favoritesWindow = new FavoritesWindow(_userId);
            favoritesWindow.Show();
        }


    }
}


