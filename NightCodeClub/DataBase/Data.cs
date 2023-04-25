using NightCodeClub.tgCommands;
using Telegram.Bot.Types;
namespace NightCodeClub.DataBase;

public interface IDataBase {
    public void Load();
    public void Save();
    public void AddNewUser(Update update);
    public AppData GetAppData();
}

public class User {
    public long chatID { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string currentRole { get; set; }
    public int rating { get; set; }
    
    public void SetRole(string role) {
        currentRole = role;
    }
}

public class AppData {
    private List<User> users {
        get;
        set;
    } = new();

    private List<RoomData> rooms {
        get;
        set;
    } = new();
    
    public void AddUser(User user) {
        users.Add(user);
        DataBridge.GetInstance().Save();
    }
    
    public void AddRoom(RoomData roomData) {
        rooms.Add(roomData);
        DataBridge.GetInstance().Save();
    }

    public List<User> GetUsers() {
        return users;
    }

    public List<RoomData> GetRooms() {
        return rooms;
    }
    
    public void Load(List<User> users, List<RoomData> rooms) {
        this.users = users;
        this.rooms = rooms;
    }
}


