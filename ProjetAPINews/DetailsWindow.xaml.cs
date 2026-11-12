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
    /// Logique d'interaction pour DetailsWindow.xaml
    /// </summary>
    public partial class DetailsWindow : Window
    {
        private NewsArticle _article;
        private NewsViewModel _viewModel;
        private int _userId;
        public DetailsWindow(NewsArticle article, NewsViewModel viewModel,int userId)
        {
            InitializeComponent();
            _article = article;
            _viewModel = viewModel;
            _userId = userId;
            DataContext = _article;
            // Vérifier si l'article est déjà dans les favoris
            if (_viewModel.IsArticleInFavorites(_article, _userId))
            {
                FavoriteButton.Content = "❌ Retirer des favoris";
            }
            else
            {
                FavoriteButton.Content = "❤️ Ajouter aux favoris";
            }
        }

        private void ToggleFavorite_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.IsArticleInFavorites(_article, _userId))
            {
                _viewModel.RemoveFromFavorites(_article, _userId);
                FavoriteButton.Content = "❤️ Ajouter aux favoris";
            }
            else
            {
                _viewModel.AddToFavorites(_article, _userId);
                FavoriteButton.Content = "❌ Retirer des favoris";
            }
        }
    }
}
