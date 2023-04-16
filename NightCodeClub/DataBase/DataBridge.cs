using Telegram.Bot.Types;
namespace NightCodeClub.DataBase; 

// Bridge pattern
public class DataBridge : IDataBase {
    private readonly IDataBase _dataBase;

    public DataBridge(IDataBase dataBase) =>_dataBase = dataBase;
    public void Load() => _dataBase.Load();
    public void Save() => _dataBase.Save();
    public void AddNewUser(Update update) => _dataBase.AddNewUser(update);
    public List<RoomData> GetRooms() => _dataBase.GetRooms();
    public AppData GetAppData() => _dataBase.GetAppData();
}
