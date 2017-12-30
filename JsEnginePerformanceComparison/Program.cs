using System.Linq;
using BenchmarkDotNet.Running;

namespace JsEnginePerformanceComparison
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var benchmarkSwitcher = BenchmarkSwitcher.FromAssembly(typeof(EngineBenchmark).Assembly);
            if (args != null && args.Any(x => x.IndexOf("all") > -1))
            {
                benchmarkSwitcher.RunAll();
                return;
            }

            benchmarkSwitcher.Run(args);
        }
    }
}