using BenchmarkCode.Models;
using BenchmarkDotNet.Attributes;

namespace BenchmarkCode.Benchmarks;

public class UserInstantiationBenchmark
{
    [Benchmark]
    public User InstantiationWithCurlyBraces()
    {
        var user = new User()
        {
            Id = 1,
            Name = "John Doe",
            LastName = "Doe",
        };

        return user;
    }

    [Benchmark]
    public User InstantiationInLines()
    {
        var user = new User();
        user.Id = 1;
        user.Name = "John Doe";
        user.LastName = "Doe";

        return user;
    }
}