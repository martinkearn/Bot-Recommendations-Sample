using RecommendationsBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationsBot.Interfaces
{
    interface ICatalogRepository
    {
        IEnumerable<CatalogItem> GetAllCatalogItems();

        IEnumerable<CatalogItem> SearchCatalogItems(string query);

        CatalogItem GetItemById(string id);

        CatalogItem GetItemByName(string name);
    }
}
