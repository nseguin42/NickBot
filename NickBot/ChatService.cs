using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using OpenAI_API;

namespace EmptyBot;

public class ChatService
{
    private readonly Engine Engine;
    private readonly OpenAIAPI API;
    private readonly ChatContext context;
    private string BotName;

    private string PromptPrefix =>
        $"""
{BotName} is a helpful, conversational AI chat bot. The following is a conversation with {BotName}. The first message is shown, followed by a time skip.
{BotName}: Hey, I'm {BotName}. How can I help?
...

""";

    private string PromptSuffix =>
        $"""

{BotName}:
""";

    private static string[] StopSequences => new[] {"\n", "Human:"};

    public ChatService(string botName, string apiKey, Engine engine)
    {
        BotName = botName;
        Engine = engine;
        API = new OpenAIAPI(apiKey, engine);
        context = new ChatContext();
    }

    public void ReceiveMessage(Message message)
    {
        context.AddMessage(message);
    }

    public async Task<Message> GetReply()
    {
        var messages = context.Messages;
        var prompt = PromptPrefix + string.Join("\n", messages) + PromptSuffix;
        Console.Write("PROMPT:\n" + prompt);
        var completionRequest = new CompletionRequest(prompt)
        {
            MaxTokens = 250,
            Temperature = 0.7,
            TopP = 0.92,
            MultipleStopSequences = StopSequences,
            NumChoicesPerPrompt = 1,
        };

        var response = await API.Completions.CreateCompletionAsync(completionRequest);
        return new Message(response.Completions[0].Text.Trim(), BotName, DateTimeOffset.Now);
    }
}
