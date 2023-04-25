using NightCodeClub.AI;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace NightCodeClub.tgCommands; 

public class GenerateCommand: ICommand {
    public async void SendMessage(Update update, CancellationToken cancellationToken) {
        if (update.Message != null) {
            var request = await ChatAI.GetInstance().GenerateProject();
            await TelegramAPI.GetInstance().GetBotClient().SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: request,
                cancellationToken: cancellationToken);
        };
    }
}