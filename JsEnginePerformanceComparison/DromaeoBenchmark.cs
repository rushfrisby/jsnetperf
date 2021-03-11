using BenchmarkDotNet.Attributes;

namespace JsEnginePerformanceComparison
{
    public class DromaeoBenchmark : EngineBenchmark
    {
        static DromaeoBenchmark()
        {
            AddTest(5, @"dromaeo\dromaeo-3d-cube.js");
            AddTest(5, @"dromaeo\dromaeo-core-eval.js");
            AddTest(25, @"dromaeo\dromaeo-object-array.js");
            AddTest(25, @"dromaeo\dromaeo-object-regexp.js");
            AddTest(25, @"dromaeo\dromaeo-object-string.js");
            AddTest(5, @"dromaeo\dromaeo-string-base64.js");
        }

        [Benchmark(Description = "dromaeo-3d-cube.js")]
        public void Cube3D()
        {
            Run("dromaeo-3d-cube.js");
        }

        [Benchmark(Description = "dromaeo-core-eval.js")]
        public void CoreEval()
        {
            Run("dromaeo-core-eval.js");
        }

        [Benchmark(Description = "dromaeo-core-eval.js")]
        public void ObjectArray()
        {
            Run("dromaeo-core-eval.js");
        }

        [Benchmark(Description = "dromaeo-object-regexp.js")]
        public void ObjectRegexp()
        {
            Run("dromaeo-object-regexp.js");
        }

        [Benchmark(Description = "dromaeo-object-string.js")]
        public void ObjectString()
        {
            Run("dromaeo-object-string.js");
        }

        [Benchmark(Description = "dromaeo-string-base64.js")]
        public void StringBase64()
        {
            Run("dromaeo-string-base64.js");
        }
    }
}