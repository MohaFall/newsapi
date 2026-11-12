using System.Configuration;

namespace Api_call
{
    // Single place that reads the private settings (NewsAPI key and MySQL connection string).
    // The values come from secrets.config (ignored by Git), never from the code.
    public static class Config
    {
        public static string NewsApiKey => ConfigurationManager.AppSettings["NewsApiKey"];

        public static string ConnectionString => ConfigurationManager.AppSettings["MySqlConnection"];

        // True when secrets.config exists and both values are filled in
        public static bool IsComplete =>
            !string.IsNullOrWhiteSpace(NewsApiKey) && !string.IsNullOrWhiteSpace(ConnectionString);
    }
}
