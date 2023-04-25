using Telegram.Bot.Types;
namespace NightCodeClub.DataBase; 

// Bridge pattern
public class DataBridge : IDataBase {
    private static IDataBase _dataBase;
    private static DataBridge _instance;
    public static DataBridge GetInstance() {
        if (_instance == null) {
            _instance = new DataBridge(_dataBase);
        }
        return _instance;
    }

    public DataBridge(IDataBase dataBase) => _dataBase = dataBase;
    public void Load() => _dataBase.Load();
    public void Save() => _dataBase.Save();
    public void AddNewUser(Update update) => _dataBase.AddNewUser(update);
    public AppData GetAppData() => _dataBase.GetAppData();
}
