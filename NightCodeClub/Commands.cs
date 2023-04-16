using NightCodeClub;
using NightCodeClub.DataBase;
using Telegram.Bot;
using static NightCodeClub.Keyboard;

public static class Commands {
    private enum CommandsType {
        start,
        run,
        room,
        approve
    }

    private static CommandsType commandsType;
    
    static void GetCommand(string messageText, long chatId, CancellationToken cancellationToken) {
        commandsType = messageText switch {
            "/start" => CommandsType.start,
            "/run" => CommandsType.run,
            "/room" => CommandsType.room,
            "/approve" => CommandsType.approve,
            _ => commandsType
        };
    }

    public static async void RoomCommand(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken, IDataBase dataBase) {
        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Choose your team",
            replyMarkup: RoomKeyboardMarkup(dataBase.GetAppData().rooms.ToArray()),
            cancellationToken: cancellationToken
        );
    } 
}
