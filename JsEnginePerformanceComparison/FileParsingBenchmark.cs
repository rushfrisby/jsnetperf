using BenchmarkDotNet.Attributes;

namespace JsEnginePerformanceComparison
{
    public class FileParsingBenchmark : EngineBenchmark
    {
        static FileParsingBenchmark()
        {
            AddTest(5, @"lib\d3.min.js");
            AddTest(5, @"lib\handlebars-v4.0.5.js");
            AddTest(5, @"lib\knockout-3.4.0.js");
            AddTest(5, @"lib\lodash.min.js");
            AddTest(5, @"lib\qunit-2.0.1.js");
            AddTest(5, @"lib\underscore-min.js");
        }

        [Benchmark(Description = "d3.min.js")]
        public void D3()
        {
            Run("d3.min.js");
        }

        [Benchmark(Description = "handlebars-v4.0.5.js")]
        public void Handlebars()
        {
            Run("handlebars-v4.0.5.js");
        }

        [Benchmark(Description = "knockout-3.4.0.js")]
        public void KnockoutJs()
        {
            Run("knockout-3.4.0.js");
        }

        [Benchmark(Description = "lodash.min.js")]
        public void Lodash()
        {
            Run("lodash.min.js");
        }

        [Benchmark(Description = "qunit-2.0.1.js")]
        public void QUnit()
        {
            Run("qunit-2.0.1.js");
        }

        [Benchmark(Description = "underscore-min.js")]
        public void Underscore()
        {
            Run("underscore-min.js");
        }
    }
}