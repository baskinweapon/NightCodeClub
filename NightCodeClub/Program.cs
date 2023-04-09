﻿using System.Diagnostics;
using NightCodeClub;
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

Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();

// Send cancellation request to stop bot
cts.Cancel();

#endregion

async void HandleCallbackQuery(Update update, CancellationToken cancellationToken) {
    var callbackQuery = update.CallbackQuery;
    
    await botClient.AnswerCallbackQueryAsync(
        callbackQueryId: callbackQuery.Id,
        text: $"Received {callbackQuery.Data}",
        cancellationToken: cancellationToken
    );
}

//Main handle from Telegram bot
async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken) {
    Debug.Assert(update.Message != null, "update.Message != null");
    
    if (update.Type == UpdateType.CallbackQuery) HandleCallbackQuery(update, cancellationToken);
    if (update.Message is not { Text: { } messageText}) return;
    if (messageText.StartsWith("/")) GetCommand(messageText, update, cancellationToken);
}

async void GetCommand(string messageText, Update update, CancellationToken cancellationToken) {
    switch (messageText) {
        case "/start":
            //Add new user to database
            if (update.Message != null) {
                dataBase.AddNewUser(update);
            }
            await botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "Hi, you can use this bot to find a challenge for you team.",
                replyMarkup: Keyboard.findInlineKeyboardMarkup,
                cancellationToken: cancellationToken);
            break;
        case "/run":
            await botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "Run command",
                cancellationToken: cancellationToken);
            break;
        case "/room":
            Commands.RoomCommand(botClient, update.Message.Chat.Id, cancellationToken);
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


Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken) {
    var ErrorMessage = exception switch {
        ApiRequestException apiRequestException
            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
        _ => exception.ToString()
    };

    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
}