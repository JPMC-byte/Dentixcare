using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class ServicioIA
{
    private readonly string _apiKey;
    private readonly HttpClient _httpClient;

    public ServicioIA(string apiKey)
    {
        if (string.IsNullOrEmpty(apiKey))
        {
            throw new ArgumentException("La clave de API no puede estar vacía.", nameof(apiKey));
        }

        _apiKey = apiKey;
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
    }

    public async Task<string> GenerarRespuesta(string prompt)
    {
        var url = "https://api.openai.com/v1/chat/completions";
        var body = new
        {
            model = "gpt-4",
            messages = new[]
            {
                new { role = "system", content = "Eres un asistente que brinda consejos de emergencia para ortodoncia." },
                new { role = "user", content = prompt }
            },
            max_tokens = 500, 
            temperature = 0.7 
        };

        var jsonBody = JsonConvert.SerializeObject(body);
        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(url, content);

        if (response.IsSuccessStatusCode)
        {
            var result = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
            return result.choices[0].message.content.ToString();
        }
        else
        {
            var errorDetails = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al conectar con la IA: {response.ReasonPhrase}. Detalles: {errorDetails}");
        }
    }
}