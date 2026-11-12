using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;

namespace Api_call
{
    public partial class FavoritesWindow : Window
    {
        public ObservableCollection<NewsArticle> Favorites { get; set; }

        private int _userId;
        public FavoritesWindow(int userId)
        {
            InitializeComponent();
            _userId = userId;
            Favorites = LoadFavorites(userId);
            DataContext = this;
        }

        private ObservableCollection<NewsArticle> LoadFavorites(int userId)
        {
            var favorites = new ObservableCollection<NewsArticle>();

            using (var conn = new MySqlConnection(Config.ConnectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT Title, Description, Url, UrlToImage, PublishedAt, Source, Category FROM favorites WHERE UserId = @userId", conn);
                cmd.Parameters.AddWithValue("@userId", userId);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        favorites.Add(new NewsArticle
                        {
                            Title = reader["Title"] as string ?? "Titre inconnu",
                            Description = reader["Description"] as string ?? "Pas de description",
                            Url = reader["Url"] as string ?? "",
                            UrlToImage = reader["UrlToImage"] as string ?? "",
                            PublishedAt = reader["PublishedAt"] != DBNull.Value ? reader.GetDateTime("PublishedAt") : DateTime.Now,
                            Source = reader["Source"] as string ?? "Source inconnue",
                            Category = reader["Category"] as string ?? "Non classé",
                            IsFavorite = true
                        });
                    }
                }
            }
            return favorites;
        }

        private void FavoritesList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is ListBox listBox && listBox.SelectedItem is NewsArticle selectedArticle)
            {
                DetailsWindow detailsWindow = new DetailsWindow(selectedArticle, new NewsViewModel(), _userId);
                detailsWindow.ShowDialog();
                RefreshFavorites(); // Rafraîchir la liste après fermeture de la fenêtre

            }
        }

        public void RefreshFavorites()
        {
            Favorites.Clear();
            foreach (var article in LoadFavorites(_userId))
            {
                Favorites.Add(article);
            }
        }

    }
}
