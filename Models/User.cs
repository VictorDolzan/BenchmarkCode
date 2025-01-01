using System.Text.Json.Serialization;

namespace BenchmarkCode.Models;

public class User
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("lastName")]
    public string LastName { get; set; }
}