using NightCodeClub.DataBase;
using NightCodeClub.Helpers;
using OpenAI_API;

namespace NightCodeClub.AI; 
using Keys;
using OpenAI.GPT3;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels;

public class ChatAI {
    private static readonly ChatAI Instance = new();
    private static readonly string RequestPath = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent +
                                                 "/AI/Requests/GPTRequest.txt";
    private static readonly string ExampleOutputPath = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent +
                                                 "/AI/Requests/ExampleAnswerAI.txt";

    private OpenAIAPI api;
    public static ChatAI GetInstance() => Instance;
    
    private ChatAI() {
        api = new OpenAIAPI(PublicKeys.ApiKey);
    }
    
    public async Task<string> GenerateNewRoomName() {
        var chat = api.Chat.CreateConversation();
        chat.AppendSystemMessage("You a famous like Elon Musk, Steeve Jobs or Bill Gates." +
                                            "Create a name of team for project like a genius" +
                                            "name need was one word and maximum 10 characters"
        );
        chat.AppendUserInput("Create a name of team for project");
        var response = await chat.GetResponseFromChatbotAsync();
        return response;
    }
    
    public async Task<string> GenerateProject() {
        var chat = api.Chat.CreateConversation();
        chat.AppendSystemMessage("You a famous like Elon Musk, Steeve Jobs or Bill Gates." +
                                 " You need generate a new task for group programmers. this task need be intresting and hard. " +
                                 "Task need to be full and have simple but not obvious solution. task need to be hard but not impossible." +
                                 "Task no need harder to finish up 24 hours. Task need to be interesting for programmers." +
                                 " Task need to be interesting for you.");
        
        var output = await File.ReadAllTextAsync(ExampleOutputPath);
        chat.AppendUserInput("Generate a new task for group programmers. this task need be intresting and hard.");
        chat.AppendExampleChatbotOutput(output);
        
        var prompt = await File.ReadAllTextAsync(RequestPath);
        chat.AppendUserInput(prompt);
        var response = await chat.GetResponseFromChatbotAsync();
        return response;
    }

}