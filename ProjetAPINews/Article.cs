using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Api_call
{
    public class NewsArticle
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string UrlToImage { get; set; }
        public DateTime PublishedAt { get; set; }
        public string Source { get; set; }
        public string Category { get; set; }
        public bool IsFavorite { get; set; }
    }
}
