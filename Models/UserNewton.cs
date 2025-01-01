using Newtonsoft.Json;

namespace BenchmarkCode.Models;

public class UserNewton
{
    [JsonProperty("id")]
    public int Id { get; set; }
    
    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("lastName")]
    public string LastName { get; set; }
}