namespace EducareSA.Services
{
    public interface ILlmService
    {
        Task<string> AskAsync(string systemPrompt, IEnumerable<(string Role, string Content)> history);
    }
}