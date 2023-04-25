using Telegram.Bot;
using Telegram.Bot.Types;

namespace NightCodeClub.tgCommands; 

public class RoleCommand: ICommand {
    
    public static string[] roles = {"Developer", "UIDesigner", "ProductManager"};
    
    public async void SendMessage(Update update, CancellationToken cancellationToken) {
        if (update.Message != null)
            await TelegramAPI.GetInstance().GetBotClient().SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "🐳Choose your role🐳",
                replyMarkup: InlineKeyboard.RoleKeyboardMarkup(roles),
                cancellationToken: cancellationToken);
    }
}