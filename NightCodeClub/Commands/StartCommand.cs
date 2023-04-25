using NightCodeClub.BotMessage;
using NightCodeClub.DataBase;
using Telegram.Bot;
using Telegram.Bot.Types;
namespace NightCodeClub.tgCommands; 

public class StartCommand: ICommand {
    public async void SendMessage(Update update, CancellationToken cancellationToken) {
        if (update.Message != null) DataBridge.GetInstance().AddNewUser(update);

        if (update.Message != null)
            await TelegramAPI.GetInstance().GetBotClient().SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: BotInputData.StartMessage,
                replyMarkup: InlineKeyboard.StartKeyboardMarkup,
                cancellationToken: cancellationToken);
    }

   
}