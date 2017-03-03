using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecommendationsBot.Models
{
    [Serializable]
    public class CatalogItem
    {
        public string id { get; set; }
        public string name { get; set; }
    }
}