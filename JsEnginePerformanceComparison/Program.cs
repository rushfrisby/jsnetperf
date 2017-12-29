using BenchmarkDotNet.Running;

namespace JsEnginePerformanceComparison
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<EngineBenchmark>();
        }
    }
}