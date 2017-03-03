using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecommendationsBot.Models
{
    public class RecommendedItem
    {
        public List<CatalogItem> items { get; set; }
        public double rating { get; set; }
        public List<string> reasoning { get; set; }
    }
}