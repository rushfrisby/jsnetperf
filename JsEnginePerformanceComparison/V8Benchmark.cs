using BenchmarkDotNet.Attributes;

namespace JsEnginePerformanceComparison
{
    public class V8Benchmark : EngineBenchmark
    {
        static V8Benchmark()
        {
            AddTest(60, @"v8\v8-crypto.js");
            AddTest(5, @"v8\v8-deltablue.js");
            AddTest(60, @"v8\v8-earley-boyer.js");
            AddTest(25, @"v8\v8-raytrace.js");
            AddTest(5, @"v8\v8-richards.js");
        }


        [Benchmark(Description = "v8-crypto.js")]
        public void Crypto()
        {
            Run("v8-crypto.js");
        }

        [Benchmark(Description = "v8-deltablue.js")]
        public void DeltaBlue()
        {
            Run("v8-deltablue.js");
        }

        [Benchmark(Description = "v8-earley-boyer.js")]
        public void EarleyBoyer()
        {
            Run("v8-earley-boyer.js");
        }

        [Benchmark(Description = "v8-raytrace.js")]
        public void Raytrace()
        {
            Run("v8-raytrace.js");
        }

        [Benchmark(Description = "v8-richards.js")]
        public void Richards()
        {
            Run("v8-richards.js");
        }
    }
}