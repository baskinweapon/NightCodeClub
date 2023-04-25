using Telegram.Bot;
using Telegram.Bot.Types;

namespace NightCodeClub.tgCommands; 

public class UndifiendCommand: ICommand {
    public void SendMessage(Update update, CancellationToken cancellationToken) {
        if (update.Message != null)
            TelegramAPI.GetInstance().GetBotClient().SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "undefined command",
                cancellationToken: cancellationToken);
    }
}