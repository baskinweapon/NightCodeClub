using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

long chatId = 0;

var botClient = new TelegramBotClient("6275824665:AAEbfSRd_OZbb18aKr01xHjq0HYNlo6ucHk");

using CancellationTokenSource cts = new ();

// StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
ReceiverOptions receiverOptions = new () {
    AllowedUpdates = Array.Empty<UpdateType>() // receive all update types
};

InlineKeyboardMarkup inlineKeyboard = new(
    new []
    {
        InlineKeyboardButton.WithCallbackData(text: "Find Team", callbackData: "/find_team"),
        InlineKeyboardButton.WithCallbackData(text: "Own Team", callbackData: "12") 
    });

botClient.StartReceiving(
    updateHandler: HandleUpdateAsync,
    pollingErrorHandler: HandlePollingErrorAsync,
    receiverOptions: receiverOptions,
    cancellationToken: cts.Token
);

var me = await botClient.GetMeAsync();

Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();

// Send cancellation request to stop bot
cts.Cancel();

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    switch (update.Type) {
        case UpdateType.CallbackQuery:
            var callbackQuery = update.CallbackQuery;
            Console.WriteLine($"Received a callback query from {callbackQuery.From.Id}.");
            await botClient.AnswerCallbackQueryAsync(
                callbackQueryId: callbackQuery.Id,
                text: $"Received {callbackQuery.Data}",
                cancellationToken: cancellationToken
            );
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: callbackQuery.Data,
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);

            break;
        case UpdateType.Message:
            break;
    }
   
    if (update.Message is not { Text: { } messageText } message)
        return;
    chatId = message.Chat.Id;
    
    Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");
    
    if (messageText == "/start") {
        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Hello, World!",
            replyMarkup: inlineKeyboard,
            cancellationToken: cancellationToken);
    } if (messageText == "/run") {
        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Run command",
            cancellationToken: cancellationToken);
    } if (messageText == "/room") {
        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "room command",
            cancellationToken: cancellationToken);
    } if (messageText == "/approve") {
        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "approve command",
            replyMarkup: inlineKeyboard,
            cancellationToken: cancellationToken);
    }
}

Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException
            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
        _ => exception.ToString()
    };

    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
}