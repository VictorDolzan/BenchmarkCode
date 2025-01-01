using BenchmarkDotNet.Attributes;

namespace BenchmarkCode.Benchmarks;

public class TernaryVsIfElseBenchmark
{
    private int? user = 42;

    [Benchmark]
    public int? UsingTernary()
    {
        return user != null ? user : null;
    }

    [Benchmark]
    public int? UsingTernaryReturningUserInsideScope()
    {
        return user != null ? 40 : null;
    }

    [Benchmark]
    public int? UsingIfElse()
    {
        if (user != null)
            return user;
        else
            return null;
    }

    [Benchmark]
    public int? UsingIfElseReturningUserOutsideScope()
    {
        if (user != null)
        {
            user = 40;
            return user;
        }
        else
        {
            return null;
        }
    }
}