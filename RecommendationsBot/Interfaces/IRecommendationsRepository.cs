using RecommendationsBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationsBot.Interfaces
{
    public interface IRecommendationsRepository
    {
        Task<RecommendedItems> GetITIItems(string id, string numberOfResults, string minimalScore);
        Task<RecommendedItems> GetFBTItems(string id, string numberOfResults, string minimalScore);
    }
}
