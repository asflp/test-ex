using System.Text.Json.Serialization;

namespace WebServerHB.ImageBbResponse;

public class ImageFromApi
{
    [JsonPropertyName("data")]
    public Data Data { get; set; }
    [JsonPropertyName("success")]
    public bool Success { get; set; }
    [JsonPropertyName("status")]
    public int Status { get; set; }
}