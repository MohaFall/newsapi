using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using NewsAPI;
using NewsAPI.Constants;
using NewsAPI.Models;

namespace Api_call
{
    public class NewsViewModel
    {
        private ObservableCollection<NewsArticle> _articles;
        public ObservableCollection<NewsArticle> Articles
        {
            get => _articles;
            set
            {
                _articles = value;
            }
        }

        private string _searchTerm;
        public string SearchTerm
        {
            get => _searchTerm;
            set
            {
                _searchTerm = value;
            }
        }

        public NewsViewModel()
        {
            Articles = new ObservableCollection<NewsArticle>();
        }

        // Returns null when everything is fine, or an error message to show to the user.
        public async Task<string> LoadNewsAsync(string query = null)
        {
            try
            {
                // The API key is read from secrets.config (ignored by Git), never written in the code
                var newsApiClient = new NewsApiClient(Config.NewsApiKey);
                var response = await newsApiClient.GetEverythingAsync(new EverythingRequest
                {
                    Q = string.IsNullOrWhiteSpace(query) ? "news" : query,
                    SortBy = SortBys.Popularity,
                    Language = Languages.FR
                });

                if (response.Status != Statuses.Ok)
                {
                    return $"Erreur NewsAPI : {response.Error.Message}";
                }

                Articles.Clear();
                foreach (var article in response.Articles)
                {
                    Articles.Add(new NewsArticle
                    {
                        Title = article.Title,
                        Description = article.Description,
                        Url = article.Url,
                        UrlToImage = article.UrlToImage,
                        PublishedAt = article.PublishedAt ?? DateTime.Now,
                        Source = article.Source.Name,
                        IsFavorite = false
                    });
                }
                return null;
            }
            catch (Exception ex)
            {
                return $"Erreur lors de la récupération des actualités : {ex.Message}";
            }
        }

        public void AddToFavorites(NewsArticle article, int id)
        {
            article.IsFavorite = !article.IsFavorite;
            SaveFavoriteToDatabase(article, id);
        }

        private void SaveFavoriteToDatabase(NewsArticle article, int userId)
        {
            using (var conn = new MySqlConnection(Config.ConnectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("INSERT INTO favorites (UserId, Title, Description, Url, UrlToImage, PublishedAt, Source, Category) VALUES (@userId, @title, @description, @url, @urlToImage, @publishedAt, @source, @category)", conn);
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@title", article.Title);
                cmd.Parameters.AddWithValue("@description", article.Description);
                cmd.Parameters.AddWithValue("@url", article.Url);
                cmd.Parameters.AddWithValue("@urlToImage", article.UrlToImage);
                cmd.Parameters.AddWithValue("@publishedAt", article.PublishedAt);
                cmd.Parameters.AddWithValue("@source", article.Source);
                cmd.Parameters.AddWithValue("@category", article.Category);
                cmd.ExecuteNonQuery();
            }
        }

        public bool IsArticleInFavorites(NewsArticle article, int userId)
        {
            using (var conn = new MySqlConnection(Config.ConnectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT COUNT(*) FROM favorites WHERE UserId = @userId AND Url = @url", conn);
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@url", article.Url);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }

        public void RemoveFromFavorites(NewsArticle article, int userId)
        {
            using (var conn = new MySqlConnection(Config.ConnectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("DELETE FROM favorites WHERE UserId = @userId AND Url = @url", conn);
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@url", article.Url);
                cmd.ExecuteNonQuery();
            }
            article.IsFavorite = false;
        }
    }
}
