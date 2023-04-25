using NightCodeClub.AI;
using NightCodeClub.DataBase;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace NightCodeClub.Commands;

public class RoomCommand: ICommand {
    public async void SendMessage(Update update, CancellationToken cancellationToken) {
        if (update.Message != null)
            await TelegramAPI.GetInstance().GetBotClient().SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "Choose your team",
                replyMarkup: InlineKeyboard.RoomKeyboardMarkup(DataBridge.GetInstance().GetAppData().GetRooms()),
                cancellationToken: cancellationToken
            );
    }
}