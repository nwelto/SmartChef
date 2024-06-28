using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace GroqApiLibrary
{
    public class GroqApiClient : IGroqApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public GroqApiClient(string apiKey)
        {
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.groq.com/")
            };
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        }

        public async Task<JsonObject?> CreateChatCompletionAsync(JsonObject request)
        {
            var response = await _httpClient.PostAsync("openai/v1/chat/completions", new StringContent(request.ToJsonString(), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonNode.Parse(content)?.AsObject();
        }

        public async IAsyncEnumerable<JsonObject?> CreateChatCompletionStreamAsync(JsonObject request)
        {
            var response = await _httpClient.PostAsync("openai/v1/chat/completions", new StringContent(request.ToJsonString(), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            var stream = await response.Content.ReadAsStreamAsync();

            using var reader = new StreamReader(stream);
            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                if (line?.StartsWith("data:") == true)
                {
                    var json = line.Substring(5).Trim();
                    if (!string.IsNullOrEmpty(json))
                    {
                        yield return JsonNode.Parse(json)?.AsObject();
                    }
                }
            }
        }
    }
}

