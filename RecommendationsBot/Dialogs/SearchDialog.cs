using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using RecommendationsBot.Interfaces;
using RecommendationsBot.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace RecommendationsBot.Dialogs
{
    [Serializable]
    public class SearchDialog : IDialog<object>
    {
        private readonly ICatalogRepository _catalogRepository;
        private string _searchQuery;

        public SearchDialog(string message)
        {
            _catalogRepository = new CatalogRepository();
            _searchQuery = message;
        }

        // Entry point to the Dialog
        public async Task StartAsync(IDialogContext context)
        {
            //get top 15 catalog items
            var catalogItems = _catalogRepository.SearchCatalogItems(_searchQuery).Take(15);

            //Create card to present choices 
            IMessageActivity messageButtons = (Activity)context.MakeMessage();
            messageButtons.Recipient = messageButtons.From;
            messageButtons.Type = "message";
            messageButtons.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            foreach (var catalogItem in catalogItems)
            {
                cardButtons.Add(new CardAction() { Value = catalogItem.name, Type = "imBack", Title = catalogItem.name });
            }
            HeroCard plCard = new HeroCard()
            {
                Title = null,
                Subtitle = string.Format($"I searched for {_searchQuery} and these were the first 15 items, which one did you want?"),
                Images = null,
                Buttons = cardButtons
            };
            messageButtons.Attachments.Add(plCard.ToAttachment());
            await context.PostAsync(messageButtons);

            // State transition - wait for 'operation choice' message from user
            context.Wait(MessageReceivedOperationChoice);
        }

        public async Task MessageReceivedOperationChoice(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;

            // State transition - add RecommendationsDialog to the stack, when done call AfterChildDialogIsDone callback
            context.Call<object>(new RecommendationsDialog(message.Text), AfterChildDialogIsDone);
        }

        private async Task AfterChildDialogIsDone(IDialogContext context, IAwaitable<object> result)
        {
            // State transition - complete this Dialog and remove it from the stack
            context.Done<object>(new object());
        }
    }
}