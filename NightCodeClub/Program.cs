using System.Diagnostics;
using NightCodeClub;
using NightCodeClub.BotMessage;
using NightCodeClub.DataBase;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

#region DataBase

var data = new JsonDataBase(); // concrete implementation
var dataBase = new DataBridge(data); // bridge
dataBase.Load();

#endregion

#region Room

var roomManger = new RoomManager(dataBase);

#endregion

#region Telegram API

var botClient = new TelegramBotClient("6275824665:AAEbfSRd_OZbb18aKr01xHjq0HYNlo6ucHk");
using CancellationTokenSource cts = new ();

// StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
ReceiverOptions receiverOptions = new () {
    AllowedUpdates = Array.Empty<UpdateType>() // receive all update types
};

botClient.StartReceiving(
    updateHandler: HandleUpdateAsync,
    pollingErrorHandler: HandlePollingErrorAsync,
    receiverOptions: receiverOptions,
    cancellationToken: cts.Token
);

var me = await botClient.GetMeAsync();
roomManger.OnNewTeemCreated += SendTeamCreatedMessage;

Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();

// Send cancellation request to stop bot
cts.Cancel();

Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken) {
    var ErrorMessage = exception switch {
        ApiRequestException apiRequestException
            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
        _ => exception.ToString()
    };

    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
}

#endregion



async void HandleCallbackQuery(Update update, CancellationToken cancellationToken) {
    var callbackQuery = update.CallbackQuery;
    
    Console.WriteLine("Callback query received " + callbackQuery.Data);
    
    await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
    if (callbackQuery.Data == Key.GetFindTeamKey()) { // if user want to find team
        if (callbackQuery.Message != null)
            Commands.RoomCommand(botClient, callbackQuery.Message.Chat.Id, cancellationToken, dataBase);
    }

    if (callbackQuery.Data == Key.GetOwnTeamKey()) { // if user want to create own team
        await botClient.SendTextMessageAsync(
            chatId: callbackQuery.Message.Chat.Id,
            text: "Send me contact of your team members. Max 4 members",
            cancellationToken: cancellationToken
        );
        
    }
    
    if (roomManger.GetRoom(callbackQuery.Data) != null) { // if user choose team
        RoomData room = (RoomData)roomManger.GetRoom(callbackQuery.Data);
        room.AddMember(callbackQuery.Message.Chat.Username, callbackQuery.Message.Chat.Id);
        dataBase.Save();
        await botClient.SendTextMessageAsync(
            chatId: callbackQuery.Message.Chat.Id,
            text: "You are in " + callbackQuery.Data,
            cancellationToken: cancellationToken
        );
    }
}

//Main handle from Telegram bot
async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken) {
    if (update.Type == UpdateType.CallbackQuery) HandleCallbackQuery(update, cancellationToken);
    if (update.Message is not { Text: { } messageText}) return;
    if (update.Message.Type == MessageType.Contact) Console.WriteLine(update.Message.Type);
    if (messageText.StartsWith("/")) GetCommand(messageText, update, cancellationToken);
}

async void GetCommand(string messageText, Update update, CancellationToken cancellationToken) {
    switch (messageText) {
        case "/start":
            if (update.Message != null) dataBase.AddNewUser(update); // add new user to database
            
            await botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: BotInputData.StartMessage,
                replyMarkup: Keyboard.StartKeyboardMarkup,
                cancellationToken: cancellationToken);
            break;
        case "/run":
            await botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "Run command",
                cancellationToken: cancellationToken);
            break;
        case "/room":
            Commands.RoomCommand(botClient, update.Message.Chat.Id, cancellationToken, dataBase);
            break;
        case "/approve":
            Debug.Assert(update.Message != null, "update.Message != null");
            await botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "approve command",
                cancellationToken: cancellationToken);
            break;
    }
}

async void SendTeamCreatedMessage(string request, RoomData room) {
    Console.WriteLine("Send task message");
    Console.BackgroundColor = ConsoleColor.Magenta;
    Console.WriteLine("Room is not null");
    Console.ResetColor();
    var ids = room.ChatIds;
    foreach (var id in ids) {
        await botClient.SendTextMessageAsync(
            chatId: id,
            text: request
        );
    }
}




