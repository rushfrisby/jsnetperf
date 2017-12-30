using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;

namespace JsEnginePerformanceComparison
{
    [Config(typeof(Config))]
    public abstract class EngineBenchmark
    {
        private class Config : ManualConfig
        {
            public Config()
            {
                Add(Job.ShortRun.WithUnrollFactor(4).WithInvocationCount(4));
                Add(MemoryDiagnoser.Default);
            }
        }

        private const int StackSizeInBytes = 10000000;
        private const string TestPath = "Tests";

        private static readonly Dictionary<string, Test> Tests = new Dictionary<string, Test>();
        private static readonly string Testrunner = File.ReadAllText(Path.Combine(TestPath, "testrunner.js"));

        static EngineBenchmark()
        {
            AddTest(5, @"dromaeo\dromaeo-3d-cube.js");
            AddTest(5, @"dromaeo\dromaeo-core-eval.js");
            AddTest(25, @"dromaeo\dromaeo-object-array.js");
            AddTest(25, @"dromaeo\dromaeo-object-regexp.js");
            AddTest(25, @"dromaeo\dromaeo-object-string.js");
            AddTest(5, @"dromaeo\dromaeo-string-base64.js");

            AddTest(60, @"v8\v8-crypto.js");
            AddTest(5, @"v8\v8-deltablue.js");
            AddTest(60, @"v8\v8-earley-boyer.js");
            AddTest(25, @"v8\v8-raytrace.js");
            AddTest(5, @"v8\v8-richards.js");

            AddTest(5, @"sunspider\sunspider-3d-morph.js");
            AddTest(5, @"sunspider\sunspider-3d-raytrace.js");
            AddTest(5, @"sunspider\sunspider-access-binary-trees.js");
            AddTest(5, @"sunspider\sunspider-access-fannkuch.js");
            AddTest(5, @"sunspider\sunspider-access-nbody.js");
            AddTest(5, @"sunspider\sunspider-access-nsieve.js");
            AddTest(5, @"sunspider\sunspider-bitops-3bit-bits-in-byte.js");
            AddTest(5, @"sunspider\sunspider-bitops-bits-in-byte.js");
            AddTest(5, @"sunspider\sunspider-bitops-bitwise-and.js");
            AddTest(5, @"sunspider\sunspider-bitops-nsieve-bits.js");
            AddTest(10, @"sunspider\sunspider-controlflow-recursive.js");
            AddTest(10, @"sunspider\sunspider-crypto-aes.js");
            AddTest(5, @"sunspider\sunspider-crypto-md5.js");
            AddTest(5, @"sunspider\sunspider-crypto-sha1.js");
            AddTest(5, @"sunspider\sunspider-date-format-tofte.js");
            AddTest(5, @"sunspider\sunspider-date-format-xparb.js");
            AddTest(5, @"sunspider\sunspider-math-cordic.js");
            AddTest(5, @"sunspider\sunspider-math-partial-sums.js");
            AddTest(5, @"sunspider\sunspider-math-spectral-norm.js");
            AddTest(5, @"sunspider\sunspider-regexp-dna.js");
            AddTest(5, @"sunspider\sunspider-string-fasta.js");
            AddTest(10, @"sunspider\sunspider-string-tagcloud.js");
            AddTest(5, @"sunspider\sunspider-string-unpack-code.js");
            AddTest(5, @"sunspider\sunspider-string-validate-input.js");

            AddTest(5, @"lib\d3.min.js");
            AddTest(5, @"lib\handlebars-v4.0.5.js");
            AddTest(5, @"lib\knockout-3.4.0.js");
            AddTest(5, @"lib\lodash.min.js");
            AddTest(5, @"lib\qunit-2.0.1.js");
            AddTest(5, @"lib\underscore-min.js");
            AddTest(5, @"lib\stopwatch.js");
        }

        private static void AddTest(int timeoutSeconds, string relativePath)
        {
            var fullName = Path.Combine(TestPath, relativePath);
            var fileInfo = new FileInfo(fullName);
            if (!fileInfo.Exists)
            {
                throw new FileNotFoundException(fullName);
            }

            var testContent = File.ReadAllText(fullName);
            var content = $"{Testrunner}{Environment.NewLine}{testContent}";

            Tests.Add(fileInfo.Name, new Test(fileInfo.Name, timeoutSeconds * 1000, content));
        }

        [ParamsSource(nameof(FileNames))]
        public string FileName { get; set; }

        public IEnumerable<string> FileNames()
        {
            foreach (var entry in Tests)
            {
                yield return entry.Key;
            }
        }

        protected abstract void RunScript(string script);

        [Benchmark]
        public void Run()
        {
            var test = Tests[FileName];
            Run(test);
        }

        private void Run(Test test)
        {
            Exception error = null;
            var slim = new ManualResetEventSlim();
            var thread = new Thread(() =>
            {
                try
                {
                    RunScript(test.Content);
                }
                catch (Exception ex)
                {
                    error = ex;
                }
                finally
                {
                    slim.Set();
                }
            }, StackSizeInBytes);

            thread.Start();

            if (!slim.Wait(test.TimeoutMilliseconds))
            {
                thread.Abort();
                throw new TimeoutException();
            }

            if (error != null)
            {
                throw error;
            }
        }
    }
}