using Microsoft.Bot.Builder.Dialogs;
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
        private string _recommendationsSourceItem;

        public RecommendationsDialog(string message)
        {
            _recommendationsSourceItem = message;
        }

        // Entry point to the Dialog
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync($"The recommendations for {_recommendationsSourceItem} are .....");

            // State transition - complete this Dialog and remove it from the stack
            context.Done<object>(new object());
        }
    }
}