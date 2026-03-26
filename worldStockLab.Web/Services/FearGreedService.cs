using System.Text.Json;

public class FearGreedService
{
    private readonly HttpClient _http;

    private int _cache = 50;
    private DateTime _lastFetch = DateTime.MinValue;

    public FearGreedService(HttpClient http)
    {
        _http = http;
    }

    public async Task<int> GetIndex()
    {
        // ⭐ 缓存10分钟（关键）
        if ((DateTime.Now - _lastFetch).TotalMinutes < 10)
            return _cache;

        try
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                "https://production.dataviz.cnn.io/index/fearandgreed/graphdata"
            );

            // ⭐ 伪装浏览器（关键）
            request.Headers.Add("User-Agent",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return _cache;

            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);

            var value = doc.RootElement
                .GetProperty("fear_and_greed")
                .GetProperty("score")
                .GetInt32();

            _cache = value;
            _lastFetch = DateTime.Now;

            return value;
        }
        catch
        {
            return _cache; // ⭐ 永不崩
        }
    }
}