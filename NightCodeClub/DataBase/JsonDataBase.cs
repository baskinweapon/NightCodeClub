using System.Text.Json;
using Telegram.Bot.Types;
using File = System.IO.File;
namespace NightCodeClub.DataBase; 

public class JsonDataBase: IDataBase {
    private readonly string fileName = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent + "/data.json";
    
    public List<User>? GetAllUsers() => appData.users;

    private AppData appData;
    public void AddNewUser(Update update) {
        if (update.Message != null) {
            
            var user = new User {
                chatID = update.Message.Chat.Id,
                UserName = update.Message.Chat.Username,
                FirstName = update.Message.Chat.FirstName,
                LastName = update.Message.Chat.LastName
            };
            
            if (appData.users.Any(s => s.chatID == user.chatID)) {
                Console.WriteLine($"User {user.UserName} already exists");
                return;
            }
            appData.users.Add(user);
            Console.WriteLine("User added " + appData.users[0].UserName);
        }
        
        Save();
    }

    public List<RoomData> GetRooms() {
        return appData.rooms;
    }

    public AppData GetAppData() {
        return appData;
    }

    public void RemoveUser(User user) {
        if (appData.users.Any(s => s.chatID == user.chatID)) appData.users.Remove(user);
    }
    
    public void Load() {
        if (!File.Exists(fileName)) {
            File.Create(fileName);
        };
        var json = File.ReadAllText(fileName);
        if (json == "") {
            appData = new AppData();
            return;
        }
        appData = JsonSerializer.Deserialize<AppData>(json) ?? throw new InvalidOperationException();
    }

    public void Save() {
        var text = JsonSerializer.Serialize<AppData>(appData);
        File.WriteAllText(fileName, text);
        
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Save data" + text);
        Console.ForegroundColor = ConsoleColor.White;
    }
    
}