using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EducareSA.Services
{
    public class OllamaLlmService : ILlmService
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _config;
        private readonly ILogger<OllamaLlmService> _logger;

        public OllamaLlmService(HttpClient http, IConfiguration config, ILogger<OllamaLlmService> logger)
        {
            _http = http;
            _config = config;
            _logger = logger;
        }

        public async Task<string> AskAsync(string systemPrompt, IEnumerable<(string Role, string Content)> history)
        {
            var baseUrl = _config["Ollama:BaseUrl"] ?? "http://localhost:11434";
            var model = _config["Ollama:Model"] ?? "llama3.2";

            var messages = new List<object>
            {
                new { role = "system", content = systemPrompt }
            };

            foreach (var (role, content) in history)
            {
                messages.Add(new { role, content });
            }

            var payload = new
            {
                model,
                messages,
                stream = false,
                options = new { temperature = 0.4 }
            };

            try
            {
                var response = await _http.PostAsJsonAsync($"{baseUrl}/api/chat", payload);

                if (!response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("Ollama returned {Status}: {Body}", response.StatusCode, body);
                    return "Sorry — I couldn't reach the assistant right now. Please try again in a moment.";
                }

                var json = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);

                if (doc.RootElement.TryGetProperty("message", out var messageEl) &&
                    messageEl.TryGetProperty("content", out var contentEl))
                {
                    return contentEl.GetString()?.Trim() ?? "Sorry, I couldn't generate a response.";
                }

                return "Sorry, I couldn't generate a response.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ollama request failed");
                return "Sorry — the assistant seems to be offline. Please make sure Ollama is running and try again.";
            }
        }
    }
}