using RecommendationsBot.Interfaces;
using RecommendationsBot.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace RecommendationsBot.Repositories
{
    [Serializable]
    public class CatalogRepository : ICatalogRepository
    {
        private readonly IEnumerable<CatalogItem> _catalogItems;

        public CatalogRepository()
        {
            //this is a very basic way of populating the list of catalog items for the sake of a simple example. In a real scenario, this would come from a database or API
            var catalogItems = new List<CatalogItem>();
            var request = HttpContext.Current.Request;
            var dataFilePath = request.PhysicalApplicationPath +"/Data/catalog.csv";
            using (var fileStream = new FileStream(dataFilePath, FileMode.Open))
            {
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        var cells = line.Split(',');
                        var catalogItem = new CatalogItem()
                        {
                            id = cells[0],
                            name = cells[1],
                        };
                        catalogItems.Add(catalogItem);
                    }
                }
            }
            _catalogItems = catalogItems.AsEnumerable();
        }

        public IEnumerable<CatalogItem> GetAllCatalogItems()
        {
            return _catalogItems;
        }

        public IEnumerable<CatalogItem> SearchCatalogItems(string query)
        {
            //this is a very basic search for the sake of a simple example. In a real scenario, a full search service would be used here such as a custom API or Azure Search
            return _catalogItems.Where(i => i.name.ToLower().Contains(query.ToLower()));
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