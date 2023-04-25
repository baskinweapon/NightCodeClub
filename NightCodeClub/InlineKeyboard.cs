using Telegram.Bot.Types.ReplyMarkups;

namespace NightCodeClub;

public static class Key {
    private static string[] emojis = { "😀", "😁", "😂", "🤣", "😃", "😄", "😅", "😆", "😉", "😊", "😋", "😎", "😍", "😘", "😗", "😙", "😚", "🙂", "🤗", "🤔", "😐", "😑", "😶", "🙄", "😏", "😣", "😥", "😮", "🤐", "😯", "😪", "😫", "😴", "😌", "😛", "😜", "😝", "🤤", "😒", "😓", "😔", "😕", "🙃", "🤑", "😲", "☹️", "🙁", "😖", "😞", "😟", "😤", "😢", "😭", "😦", "😧", "😨", "😩", "😬", "😰", "😱", "😳", "🤪", "😵", "😷", "🤒", "🤕", "🥶", "🥵", "🥴", "😈", "👿", "💀", "👻", "👽", "🤖", "💩", "👶", "👦", "👧", "👨", "👩", "🧑", "👱‍♀️", "👱‍♂️", "👴", "👵", "🙍‍♂️", "🙍‍♀️", "🙎‍♂️", "🙎‍♀️", "🙅‍♂️", "🙅‍♀️", "🙆‍♂️", "🙆‍♀️", "💁‍♂️", "💁‍♀️", "🙋‍♂️", "🙋‍♀️", "🤦‍♂️", "🤦‍♀️", "🤷‍♂️", "🤷‍♀️", "💆‍♂️", "💆‍♀️", "💇‍♂️", "💇‍♀️", "🚶‍♂️", "🚶‍♀️", "🏃‍♂️", "🏃‍♀️", "💃", "🕺", "👯‍♂️", "👯‍♀️", "🧖‍♂️", "🧖‍♀️", "👩‍❤️‍👩", "👨‍❤️‍👨", "💏", "👩‍❤️‍💋‍👩", "👨‍❤️‍💋‍👨", "💑", "👩‍❤️‍💋‍👨"}; 
    
    public static string GetRoomKey() => emojis[0];
    public static string GetRulesKey() => emojis[1];
    public static string GetApplyKey() => emojis[2];
    public static string GetBackKey() => emojis[3];
    
}

public static class InlineKeyboard {
    public static readonly InlineKeyboardMarkup StartKeyboardMarkup = new(
        new []
        {
            InlineKeyboardButton.WithCallbackData(text: "Projects", callbackData: Key.GetRoomKey()),
            InlineKeyboardButton.WithCallbackData(text: "Rules", callbackData: Key.GetRulesKey())
        });
    
    
    public static InlineKeyboardMarkup ProjectInKeyboardMarkup(RoomData room) {
         var inlineKeyboard = new []
        {
            InlineKeyboardButton.WithCallbackData(text: "Apply", callbackData: Key.GetApplyKey()),
            InlineKeyboardButton.WithCallbackData(text: "Back", callbackData: Key.GetBackKey())
        };
        return inlineKeyboard;
    }
    
   
    
    public static InlineKeyboardMarkup RoleKeyboardMarkup(string[] roles) {
        var inlineKeyboard = new InlineKeyboardButton[roles.Length][];
        for (var i = 0; i < inlineKeyboard.Length; i++) {
            inlineKeyboard[i] =  new InlineKeyboardButton[] {
                InlineKeyboardButton.WithCallbackData(text: roles[i], callbackData: roles[i])
            };
        }
        return inlineKeyboard;
    }
    
    public static InlineKeyboardMarkup RoomKeyboardMarkup(List<RoomData> roomData) {
        var inlineKeyboard = new InlineKeyboardButton[roomData.Count];
        for (var i = 0; i < roomData.Count; i++) {
            inlineKeyboard[i] = InlineKeyboardButton.WithCallbackData(text: roomData[i].name, callbackData: roomData[i].name);
        }
        return inlineKeyboard;
    }
}