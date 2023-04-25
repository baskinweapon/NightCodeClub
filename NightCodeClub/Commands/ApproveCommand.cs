using Telegram.Bot;
using Telegram.Bot.Types;

namespace NightCodeClub.tgCommands; 

public class ApproveCommand: ICommand {
    public async void SendMessage(Update update, CancellationToken cancellationToken) {
        if (update.Message != null)
            await TelegramAPI.GetInstance().GetBotClient().SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "approve command",
                cancellationToken: cancellationToken);
    }
}