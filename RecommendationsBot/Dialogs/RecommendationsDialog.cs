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
        private CatalogItem _sourceCatalogItem;

        public RecommendationsDialog(string message)
        {
            _catalogRepository = new CatalogRepository();
            _sourceCatalogItem = _catalogRepository.GetItemByName(message);
        }

        // Entry point to the Dialog
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync($"The recommendations for {_sourceCatalogItem.name} are .....");

            // State transition - complete this Dialog and remove it from the stack
            context.Done<object>(new object());
        }
    }
}