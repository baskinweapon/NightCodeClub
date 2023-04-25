using NightCodeClub.Commands;
using NightCodeClub.DataBase;
using NightCodeClub.tgCommands;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace NightCodeClub; 

public class Query {
    public async void CallbackData(CallbackQuery? callbackQuery, TelegramBotClient botClient, CancellationToken cancellationToken) {
        if (callbackQuery?.Data == Key.GetRoomKey()) {
            await botClient.SendTextMessageAsync(
                chatId: callbackQuery.Message.Chat.Id,
                text: "Jump in Project",
                replyMarkup: InlineKeyboard.RoomKeyboardMarkup(DataBridge.GetInstance().GetAppData().GetRooms()),
                cancellationToken: cancellationToken
            );
        }
        
        var roles = RoleCommand.roles.find(callbackQuery?.Data);
        if (roles) {
            var user = DataBridge.GetInstance().GetAppData().GetUsers().Find(x => x.chatID == callbackQuery.Message.Chat.Id);
            user?.SetRole(callbackQuery.Data);
            DataBridge.GetInstance().Save();
            await botClient.SendTextMessageAsync(
                chatId: callbackQuery.Message.Chat.Id,
                text: "Your role " + callbackQuery.Data,
                cancellationToken: cancellationToken
            );
        }
        
        if (RoomManager.GetInstance().GetRoom(callbackQuery.Data) != null) {
            // if user choose team
            var room = RoomManager.GetInstance().GetRoom(callbackQuery.Data);
            if (callbackQuery.Message != null)
                await botClient.SendTextMessageAsync(
                    chatId: callbackQuery.Message.Chat.Id,
                    text: room.projectDescription,
                    replyMarkup: InlineKeyboard.ProjectInKeyboardMarkup(room),
                    cancellationToken: cancellationToken
                );
        }
        
        if (callbackQuery?.Data == Key.GetApplyKey()) {
            await botClient.SendTextMessageAsync(
                chatId: callbackQuery.Message.Chat.Id,
                text: "You are in the project ",
                cancellationToken: cancellationToken
            );
        }
    }
}