using System.Linq;
using BenchmarkDotNet.Running;

namespace JsEnginePerformanceComparison
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var benchmarkSwitcher = BenchmarkSwitcher.FromAssembly(typeof(EngineBenchmark).Assembly);
            if (args.Any(x => x == "--all"))
            {
                benchmarkSwitcher.RunAllJoined();
            }
            else
            {
                benchmarkSwitcher.Run();
            }
        }
    }
}