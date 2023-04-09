using System.Text.Json;
using Telegram.Bot.Types;
using File = System.IO.File;
namespace NightCodeClub.DataBase; 

public class JsonDataBase: IDataBase {
    private readonly string fileName = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent + "/data.json";
    
    public List<User>? GetAllUsers() => users;
    
    private List<User> users;
    public void AddNewUser(Update update) {
        if (update.Message != null) {
            
            var user = new User {
                chatID = update.Message.Chat.Id,
                UserName = update.Message.Chat.Username,
                FirstName = update.Message.Chat.FirstName,
                LastName = update.Message.Chat.LastName
            };
            
            if (users.Any(s => s.chatID == user.chatID)) {
                Console.WriteLine($"User {user.UserName} already exists");
                return;
            }
            users.Add(user);
        }

        Save();
    }
    
    public void RemoveUser(User user) {
        if (users.Any(s => s.chatID == user.chatID)) users.Remove(user);
    }
    
    public void Load() {
        if (!File.Exists(fileName)) {
            File.Create(fileName);
        };
        var json = File.ReadAllText(fileName);
        if (json == "") {
            users = new List<User>();
            return;
        }
        users = JsonSerializer.Deserialize<List<User>>(json) ?? throw new InvalidOperationException();
        foreach (var user in users) {
            Console.WriteLine(user.chatID);
            Console.WriteLine(user.UserName);
            Console.WriteLine(user.FirstName);
            Console.WriteLine(user.LastName);
        }
    }

    public void Save() {
        var text = JsonSerializer.Serialize<List<User>>(users);
        Console.WriteLine("text = " + text);
        File.WriteAllText(fileName, text);
    }
    
}