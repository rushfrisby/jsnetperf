using BenchmarkDotNet.Attributes;

namespace JsEnginePerformanceComparison
{
    public class StopwatchBenchmark : EngineBenchmark
    {
        static StopwatchBenchmark()
        {
            AddTest(5, @"lib\stopwatch.js");
        }

        [Benchmark(Description = "stopwatch.js")]
        public void Stopwatch()
        {
            Run("stopwatch.js");
        }
    }
}