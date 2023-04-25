using NightCodeClub.Commands;
using NightCodeClub.Helpers;
using NightCodeClub.Keys;
using NightCodeClub.tgCommands;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace NightCodeClub; 

public class TelegramAPI {
    private static TelegramAPI Instance { get; } = new();
    
    private TelegramBotClient botClient;
    private CancellationTokenSource cts = new ();
    private ReceiverOptions receiverOptions = new () {AllowedUpdates = Array.Empty<UpdateType>()};

    public static  TelegramAPI GetInstance() => Instance;
    public TelegramBotClient GetBotClient() => botClient;
    private TelegramAPI() {
        botClient = new TelegramBotClient(PublicKeys.TelegramBotKey);
        
        botClient.StartReceiving(
            updateHandler: HandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync,
            receiverOptions: receiverOptions,
            cancellationToken: cts.Token
            );
    }
    
    public async void GetMeAsync() => await botClient.GetMeAsync();
    
    //Main handle from Telegram bot
    async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken) {
        if (update.Type == UpdateType.CallbackQuery) HandleCallbackQuery(update, cancellationToken);
        if (update.Message is not { Text: { } messageText}) return;
        if (update.Message.Type == MessageType.Contact) Console.WriteLine(update.Message.Type);
        if (messageText.StartsWith("/")) CommandDistribution(messageText, update, cancellationToken);
    }

    public void CommandDistribution(string messageText, Update update, CancellationToken cancellationToken) {
        messageText = messageText.Replace("/", "");
        ICommand command = messageText switch {
            "start" => new StartCommand(),
            "generate" => new GenerateCommand(),
            "approve" => new ApproveCommand(),
            "role" => new RoleCommand(),
            "room" => new RoomCommand(),
            _ => new UndifiendCommand()
        };
        command.SendMessage(update, cancellationToken);
    }
    async void HandleCallbackQuery(Update update, CancellationToken cancellationToken) {
        var callbackQuery = update.CallbackQuery;
        await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, cancellationToken: cancellationToken);

        var query = new Query();
        query.CallbackData(callbackQuery, botClient, cancellationToken);
    
    }

    private Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken) {
        var errorMessage = exception switch {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };
        Console.WriteLine(errorMessage);
        return Task.CompletedTask;
    }
    
    public async void StartListening() {
        var me = await botClient.GetMeAsync();
        Log.W("In the mirror - a new world." +
                  $"\nThe @{me.Username} is a dreamy seeker." +
                  "\nI greet the light and you.", ConsoleColor.Yellow, ConsoleColor.Blue);
    }
    
    public void StopBot() => cts.Cancel();
}
