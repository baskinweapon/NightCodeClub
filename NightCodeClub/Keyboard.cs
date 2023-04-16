using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace NightCodeClub;

public class Key {
    private static string[] emojis = { "😀", "😁", "😂", "🤣", "😃", "😄", "😅", "😆", "😉", "😊", "😋", "😎", "😍", "😘", "😗", "😙", "😚", "🙂", "🤗", "🤔", "😐", "😑", "😶", "🙄", "😏", "😣", "😥", "😮", "🤐", "😯", "😪", "😫", "😴", "😌", "😛", "😜", "😝", "🤤", "😒", "😓", "😔", "😕", "🙃", "🤑", "😲", "☹️", "🙁", "😖", "😞", "😟", "😤", "😢", "😭", "😦", "😧", "😨", "😩", "😬", "😰", "😱", "😳", "🤪", "😵", "😷", "🤒", "🤕", "🥶", "🥵", "🥴", "😈", "👿", "💀", "👻", "👽", "🤖", "💩", "👶", "👦", "👧", "👨", "👩", "🧑", "👱‍♀️", "👱‍♂️", "👴", "👵", "🙍‍♂️", "🙍‍♀️", "🙎‍♂️", "🙎‍♀️", "🙅‍♂️", "🙅‍♀️", "🙆‍♂️", "🙆‍♀️", "💁‍♂️", "💁‍♀️", "🙋‍♂️", "🙋‍♀️", "🤦‍♂️", "🤦‍♀️", "🤷‍♂️", "🤷‍♀️", "💆‍♂️", "💆‍♀️", "💇‍♂️", "💇‍♀️", "🚶‍♂️", "🚶‍♀️", "🏃‍♂️", "🏃‍♀️", "💃", "🕺", "👯‍♂️", "👯‍♀️", "🧖‍♂️", "🧖‍♀️", "👩‍❤️‍👩", "👨‍❤️‍👨", "💏", "👩‍❤️‍💋‍👩", "👨‍❤️‍💋‍👨", "💑", "👩‍❤️‍💋‍👨"}; 
    private static string findTeam;
    private string ownTeam;
    
    public static string GetFindTeamKey() {
        return emojis[0];
    }
    
    public static string GetOwnTeamKey() {
        return emojis[1];
    }
}

public static class Keyboard {
    public static readonly InlineKeyboardMarkup StartKeyboardMarkup = new(
        new []
        {
            InlineKeyboardButton.WithCallbackData(text: "Find Team", callbackData: Key.GetFindTeamKey()),
            InlineKeyboardButton.WithCallbackData(text: "Own Team", callbackData: Key.GetOwnTeamKey())
        });
    
    public static InlineKeyboardMarkup RoomKeyboardMarkup(RoomData[] roomData) {
        var inlineKeyboard = new InlineKeyboardButton[roomData.Length];
        for (int i = 0; i < roomData.Length; i++) {
            inlineKeyboard[i] = InlineKeyboardButton.WithCallbackData(text: roomData[i].name, callbackData: roomData[i].name);
        }
        return inlineKeyboard;
    }
}