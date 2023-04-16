using Telegram.Bot.Types;
namespace NightCodeClub.DataBase;

public interface IDataBase {
    public void Load();
    public void Save();
    public void AddNewUser(Update update);
    public List<RoomData> GetRooms();
    public AppData GetAppData();
}

public class User {
    public long chatID { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

public class AppData {
    public List<User> users { get; set; } = new();
    public List<RoomData> rooms { get; set; } = new();
}


