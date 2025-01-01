using BenchmarkCode.Models;
using BenchmarkDotNet.Attributes;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace BenchmarkCode.Benchmarks;

public class JsonConverterBenchmark
{
    [Benchmark]
    public string SerializeSystemText()
    {
        var user = new User()
        {
            Id = 1,
            Name = "John Doe",
            LastName = "Doe"
        };

        return JsonSerializer.Serialize(user);
    }

    [Benchmark]
    public string SerializeNewton()
    {
        var user = new UserNewton()
        {
            Id = 1,
            Name = "John Doe",
            LastName = "Doe"
        };

        return JsonConvert.SerializeObject(user);
    }
}