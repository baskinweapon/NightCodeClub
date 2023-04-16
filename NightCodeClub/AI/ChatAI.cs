namespace NightCodeClub.AI; 
using OpenAI.GPT3;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels;

public static class ChatAI {
    private static string apiKey = "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";
    public static Action<string> OnGenerateTask;
    public static async void Request(string prompt) {
        var gpt3 = new OpenAIService(new OpenAiOptions() {
            ApiKey = apiKey
        });
        
        var completionResult = await gpt3.Completions.CreateCompletion(new CompletionCreateRequest() {
            Prompt = prompt,
            Model = Models.TextDavinciV2,
            Temperature = 0.4F,
            MaxTokens = 100,
            N = 2
        });
        
        if (completionResult.Successful) {
            foreach (var choice in completionResult.Choices) {
                Console.WriteLine(choice.Text);
                return;
            }                
        } else {
            if (completionResult.Error == null) {
                throw new Exception("Unknown Error");
            }
            Console.WriteLine($"{completionResult.Error.Code}: {completionResult.Error.Message}");
        }
    }
}