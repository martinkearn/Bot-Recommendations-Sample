using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
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
        private string _searchQuery;

        public SearchDialog(string message)
        {
            _searchQuery = message;
        }

        // Entry point to the Dialog
        public async Task StartAsync(IDialogContext context)
        {
            //get choices
            var choices = new List<string>() { "1", "2", "3" };

            //Create card to present choices 
            IMessageActivity messageButtons = (Activity)context.MakeMessage();
            messageButtons.Recipient = messageButtons.From;
            messageButtons.Type = "message";
            messageButtons.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            foreach (var choice in choices)
            {
                cardButtons.Add(new CardAction() { Value = choice, Type = "imBack", Title = choice });
            }
            HeroCard plCard = new HeroCard()
            {
                Title = null,
                Subtitle = string.Format($"I searched for {_searchQuery} and found these items, which one did you want?"),
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

            // State transition - add 'recommendations' Dialog to the stack, when done call AfterChildDialogIsDone callback
            context.Call<object>(new RecommendationsDialog(message.Text), AfterChildDialogIsDone);
        }

        private async Task AfterChildDialogIsDone(IDialogContext context, IAwaitable<object> result)
        {
            // State transition - complete this Dialog and remove it from the stack
            context.Done<object>(new object());
        }
    }
}