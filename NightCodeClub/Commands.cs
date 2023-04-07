using NightCodeClub;
using Telegram.Bot;
using Telegram.Bot.Types;
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

    public static async void RoomCommand(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken) {
        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Waiting another player...",
            replyMarkup: RoomKeyboardMarkup(),
            cancellationToken: cancellationToken);
        
    } 
}
