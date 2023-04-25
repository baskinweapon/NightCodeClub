using System.Collections;
using System.Text.Json;
using NightCodeClub.Helpers;
using Telegram.Bot.Types;
using File = System.IO.File;
namespace NightCodeClub.DataBase;


public class JsonDataBase: IDataBase {
   
    private static readonly string fileName = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent + "/data.json";
    
    private AppData appData;
    public void AddNewUser(Update update) {
        if (update.Message != null) {
            var user = new User {
                chatID = update.Message.Chat.Id,
                UserName = update.Message.Chat.Username,
                FirstName = update.Message.Chat.FirstName,
                LastName = update.Message.Chat.LastName
            };
            
            if (appData.GetUsers().Any(s => s.chatID == user.chatID)) {
                Console.WriteLine($"User {user.UserName} already exists");
                return;
            }
            appData.AddUser(user);
            Console.WriteLine("User added " + appData.GetUsers()[0].UserName);
        }
    }
    
    public AppData GetAppData() {
        return appData;
    }

    public void RemoveUser(User user) {
        if (appData.GetUsers().Any(s => s.chatID == user.chatID)) appData.GetUsers().Remove(user);
    }
    
    public void Load() {
        if (!File.Exists(fileName)) {
            File.Create(fileName);
        };
        appData = new AppData();
        var json = File.ReadAllText(fileName);
        if (json == "") return;
        var load = JsonSerializer.Deserialize<AppDataJson>(json);
        load.users ??= new List<User>();
        load.rooms ??= new List<RoomData>();
        appData.Load(load.users, load.rooms);
    }

    public void Save() {
        var save = new AppDataJson(appData.GetRooms(), appData.GetUsers());
        var text = JsonSerializer.Serialize(save, options: new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(fileName, text);
        
        Log.W("Save data", ConsoleColor.Green, ConsoleColor.DarkGray);
    }
    
    private struct AppDataJson {
        public List<RoomData> rooms { get; set; } = new();
        public List<User> users { get; set; } = new();

        public AppDataJson(List<RoomData> rooms, List<User> users) {
            this.rooms = rooms;
            this.users = users;
        }
    }
    
}