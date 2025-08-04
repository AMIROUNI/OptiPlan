using OptiPlanBackend.Services.Interfaces;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OptiPlanBackend.Services
{
    public class ChatBotService : IChatBotService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public ChatBotService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _apiKey = Environment.GetEnvironmentVariable("API_KEY_OPENROUTER")
                ?? throw new ArgumentNullException("Missing API_KEY_OPENROUTER in .env");
        }

        public async Task<string> AskBotAsync(string userMessage)
        {
            var requestBody = new
            {
                model = "openai/gpt-3.5-turbo",
                messages = new[]
      {
        new { role = "user", content = userMessage }
    }
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Clear(); // important !
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _apiKey);
            _httpClient.DefaultRequestHeaders.Add("HTTP-Referer", "http://localhost:4200");
            _httpClient.DefaultRequestHeaders.Add("X-Title", "OptiPlanBot");

            var response = await _httpClient.PostAsync("https://openrouter.ai/api/v1/chat/completions", content);

            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                // Affiche le message d’erreur pour t’aider à debugger
                throw new Exception($"OpenRouter Error {response.StatusCode}: {responseBody}");
            }

            using var doc = JsonDocument.Parse(responseBody);
            var botReply = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return botReply ?? "No response.";
        }

    }
}
