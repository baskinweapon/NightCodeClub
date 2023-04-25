using NightCodeClub.AI;
using NightCodeClub.DataBase;
using NightCodeClub.Helpers;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = NightCodeClub.DataBase.User;

namespace NightCodeClub; 



public class RoomData {
    public string name {get; set; }
    public int membersCount {get; set; }
    public List<User> users { get; set; }
    public string projectDescription { get; set; }

    public void AddMember(Chat chat) {
        if (DataBridge.GetInstance().GetAppData().GetUsers().Any(user => user.chatID == chat.Id)) 
            return;
        membersCount++;
        foreach (var user in DataBridge.GetInstance().GetAppData().GetUsers().Where(user => user.chatID == chat.Id)) {
            users.Add(user);
        }
    }
    
    public RoomData(string name, List<User> users, string projectDescription, int membersCount = 0) {
        this.name = name;
        this.membersCount = membersCount;
        this.users = users;
        this.projectDescription = projectDescription;
    }

    public List<User> GetMembers() {
        return users;
    }
}

public class RoomManager {
    private static RoomManager instance = new();
    
    public static RoomManager GetInstance() {
        return instance;
    }
    
    private async void CreateTeam(RoomData roomData) {
        Log.W($"Team {roomData.name} created. \nMembers", ConsoleColor.Green);
        
        var taskRequest = ChatAI.GetInstance().GenerateProject();
        var task = await taskRequest;
        Log.W(task, ConsoleColor.DarkGreen);
        
        ProjectCreatedMessage(task, roomData);
    }
    
    async void ProjectCreatedMessage(string request, RoomData room) {
        var ids = room.GetMembers().Select(user => user.chatID).ToArray();
        foreach (var id in ids) {
            await TelegramAPI.GetInstance().GetBotClient().SendTextMessageAsync(
                chatId: id,
                text: request
            );
        }
    }

    public async void GenerateNewRoom() {
        Log.W("Generating room ...", ConsoleColor.Magenta);
        var appData = DataBridge.GetInstance().GetAppData();
        var generatedRoomName = await ChatAI.GetInstance().GenerateNewRoomName();
        var request = await ChatAI.GetInstance().GenerateProject();
        var roomData = new RoomData(generatedRoomName, new List<User>(), request);
        appData.AddRoom(roomData);
    }
    
    public RoomData? GetRoom(string roomName) {
        return DataBridge.GetInstance().GetAppData().GetRooms().FirstOrDefault(room => room.name == roomName);
    }
}