using Microsoft.Bot.Builder.Dialogs;
using RecommendationsBot.Interfaces;
using RecommendationsBot.Models;
using RecommendationsBot.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace RecommendationsBot.Dialogs
{
    [Serializable]
    public class RecommendationsDialog : IDialog<object>
    {
        private readonly ICatalogRepository _catalogRepository;
        private readonly IRecommendationsRepository _recommendationsRepository;
        private CatalogItem _sourceCatalogItem;

        public RecommendationsDialog(string message)
        {
            _catalogRepository = new CatalogRepository();
            _recommendationsRepository = new RecommendationsRepository();
            _sourceCatalogItem = _catalogRepository.GetItemByName(message);
        }

        // Entry point to the Dialog
        public async Task StartAsync(IDialogContext context)
        {
            var fbtItems = await _recommendationsRepository.GetFBTItems(_sourceCatalogItem.id, "15", "0");

            if (fbtItems.recommendedItems.Count > 0)
            {
                await context.PostAsync($"The recommendations for {_sourceCatalogItem.name} are .....");
                foreach (var fbtItem in fbtItems.recommendedItems)
                {
                    await context.PostAsync($"{fbtItem.items.FirstOrDefault().name} with a score of {fbtItem.rating}");
                }
            }
            else
            {
                await context.PostAsync($"Sorry, could not find any recommendations for {_sourceCatalogItem.name}");
            }

            // State transition - complete this Dialog and remove it from the stack
            context.Done<object>(new object());
        }
    }
}