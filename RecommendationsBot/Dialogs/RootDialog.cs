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
    public class RootDialog : IDialog<object>
    {
        // Entry point to the Dialog
        public async Task StartAsync(IDialogContext context)
        {
            // State transition - wait for 'start' message from user
            context.Wait(MessageReceivedStart);
        }

        public async Task MessageReceivedStart(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            await context.PostAsync($"Give me a book title to search for. Try 'Lord of the Flies'");

            // State transition - wait for 'search query' message from user
            context.Wait(MessageReceivedOperationChoice);
        }

        public async Task MessageReceivedOperationChoice(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;

            // State transition - add SearchDialog to the stack, when done call AfterChildDialogIsDone callback
            context.Call<object>(new SearchDialog(message.Text), AfterChildDialogIsDone);
        }

        private async Task AfterChildDialogIsDone(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync($"To start again, give me another book title to search for. Try 'The Great Gatsby'");

            // State transition - wait for 'search query' message from user
            context.Wait(MessageReceivedOperationChoice);
        }
    }
}