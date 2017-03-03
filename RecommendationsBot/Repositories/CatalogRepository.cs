using RecommendationsBot.Interfaces;
using RecommendationsBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecommendationsBot.Repositories
{
    [Serializable]
    public class CatalogRepository : ICatalogRepository
    {
        private readonly IEnumerable<CatalogItem> _catalogItems;

        public CatalogRepository()
        {
            var catalogItems = new List<CatalogItem>();
            catalogItems.Add(new CatalogItem() { id = "1", name = "Item 1" });
            catalogItems.Add(new CatalogItem() { id = "2", name = "Item 2" });
            catalogItems.Add(new CatalogItem() { id = "3a", name = "Item 3 A" });
            catalogItems.Add(new CatalogItem() { id = "3b", name = "Item 3 B" });
            catalogItems.Add(new CatalogItem() { id = "4", name = "Item 4" });
            _catalogItems = catalogItems.AsEnumerable();
        }

        public IEnumerable<CatalogItem> GetAllCatalogItems()
        {
            return _catalogItems;
        }

        public IEnumerable<CatalogItem> SearchCatalogItems(string query)
        {
            return _catalogItems.Where(i => i.name.ToLower().Contains(query.ToLower())).AsEnumerable();
        }

        public CatalogItem GetItemById(string id)
        {
            return _catalogItems.Where(i => i.id == id).FirstOrDefault();
        }

        public CatalogItem GetItemByName(string name)
        {
            return _catalogItems.Where(i => i.name == name).FirstOrDefault();
        }
    }
}