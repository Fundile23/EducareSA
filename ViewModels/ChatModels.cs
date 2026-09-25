namespace EducareSA.ViewModels
{
    public class ChatMessage
    {
        public string Role { get; set; } = "user"; // "user" | "assistant" | "system"
        public string Content { get; set; } = string.Empty;
    }

    public class ChatRequest
    {
        public string Message { get; set; } = string.Empty;
    }

    public class ChatResponse
    {
        public string Reply { get; set; } = string.Empty;
        public bool Success { get; set; } = true;
        public string? Error { get; set; }
    }
}