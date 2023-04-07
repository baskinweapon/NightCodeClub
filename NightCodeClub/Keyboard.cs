using Telegram.Bot.Types.ReplyMarkups;

namespace NightCodeClub;

public static class Keyboard {
    public static readonly InlineKeyboardMarkup findInlineKeyboardMarkup = new(
        new []
        {
            InlineKeyboardButton.WithCallbackData(text: "Find Team", callbackData: "/find_team"),
            InlineKeyboardButton.WithCallbackData(text: "Own Team", callbackData: "/own_team")
        });
    
    public static InlineKeyboardMarkup RoomKeyboardMarkup() {
        
        InlineKeyboardMarkup inlineKeyboard = new(
            new []
            {
                InlineKeyboardButton.WithCallbackData(text: "!!!", callbackData: "/!!!"),
                InlineKeyboardButton.WithCallbackData(text: "!!!", callbackData: "/!!!")
            });
        return inlineKeyboard;
    }
   
}