// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with EmptyBot .NET Template version v4.17.1

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using OpenAI_API;

namespace EmptyBot
{
    public class Bot : ActivityHandler
    {
        private static readonly string BotName = Environment.GetEnvironmentVariable("BOT_NAME");
        private static readonly string ApiKey = Environment.GetEnvironmentVariable("API_KEY");
        private static readonly Engine Engine = Engine.Davinci;
        private static readonly ChatService ChatService = new ChatService(BotName, ApiKey, Engine);

        protected override async Task OnMessageActivityAsync(
            ITurnContext<IMessageActivity> turnContext,
            CancellationToken cancellationToken)
        {
            var messageText = turnContext.Activity.Text;
            var name = turnContext.Activity.From.Name;
            var time = turnContext.Activity.Timestamp;
            var message = new Message(messageText, name, time ?? DateTimeOffset.Now);

            ChatService.ReceiveMessage(message);

            var response = await ChatService.GetReply();
            await turnContext.SendActivityAsync(
                response.Text,
                cancellationToken: cancellationToken);
            ChatService.ReceiveMessage(response);
        }
    }
}
