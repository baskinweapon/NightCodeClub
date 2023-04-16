using System.Collections;
using System.Diagnostics;
using NightCodeClub.AI;
using NightCodeClub.DataBase;

namespace NightCodeClub; 

public class RoomData {
    public Action<RoomData> OnFullRoom;
    
    public string name {get; set; }
    public int membersCount {get; set; }
    public List<string> userNames {get; set; }
    public List<long> ChatIds { get; set; }
    
    public void AddMember(string userName, long chatId) {
        if (ChatIds.Contains(chatId)) return;
        if (membersCount > 2) return;
        membersCount++;
        if (membersCount == 1) OnFullRoom.Invoke(this);
            userNames.Add(userName);
        ChatIds.Add(chatId);
    }
    
    public RoomData(string name, List<string> userNames, List<long> chatIds, int membersCount = 0) {
        this.name = name;
        this.membersCount = membersCount;
        this.userNames = userNames;
        ChatIds = chatIds;
    }

    public List<string> GetMembers() {
        return userNames;
    }
}

public class RoomManager {
    private AppData appData;
    private IDataBase dataBase;
    
    public RoomManager(IDataBase dataBase) {
        this.dataBase = dataBase;
        appData = dataBase.GetAppData();
        if (appData.rooms.Count == 0) {
            appData.rooms = new List<RoomData>();
            GenerateNewRoom();
        }
        
        foreach (var room in appData.rooms) {
            room.OnFullRoom += CreateTeam;
        }
    }

    public Action<string, RoomData> OnNewTeemCreated { get; set; }

    private async void CreateTeam(RoomData roomData) {
        Console.WriteLine($"Team {roomData.name} created. \nMembers");
        
        // include AI and create Task
        var taskRequest = ChatAI.Request(
            $"Create a task for team {roomData.name} with members super weapon," +
            $"task need to be done in 24 hours." +
            $"task need to be C# code and Unity." +
            $"for task needed have hard skills." +
            $"task need to be helphull for social.");
        
        var task = await taskRequest;
        Console.WriteLine(task);
        OnNewTeemCreated?.Invoke(task, roomData);
    }
    
    public void GenerateNewRoom() {
        string generatedRoomName = "Room " + appData.rooms.Count;
        var roomData = new RoomData(generatedRoomName, new List<string>(), new List<long>());
        appData.rooms.Add(roomData);
        Console.WriteLine(appData.rooms[0].name);
        dataBase.Save();
    }
    
    public RoomData? GetRoom(string roomName) {
        foreach (var room in appData.rooms) {
            if (room.name == roomName) {
                return room;
            }
        }
        return null;
    }
    
    public void FindRoom(string userName) {
        
    }
}