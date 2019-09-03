using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace JsEnginePerformanceComparison
{
    public enum Engine
    {
        ClearScriptV8,
        IronJs,
        Jint,
        Jurassic,
        NilJs
    }

    [Config(typeof(Config))]
    [MemoryDiagnoser]
    public class EngineBenchmark
    {
        private class Config : ManualConfig
        {
            public Config() => Orderer = new EngineBenchmarkOrderer();

            private class EngineBenchmarkOrderer : IOrderer
            {
                public IEnumerable<BenchmarkCase> GetExecutionOrder(ImmutableArray<BenchmarkCase> benchmarksCase) =>
                    from benchmark in benchmarksCase
                    orderby benchmark.Descriptor.DisplayInfo
                    select benchmark;

                public IEnumerable<BenchmarkCase> GetSummaryOrder(ImmutableArray<BenchmarkCase> benchmarksCase, Summary summary) =>
                    from benchmark in benchmarksCase
                    orderby benchmark.Descriptor.DisplayInfo, benchmark.Parameters["ReuseEngine"], summary[benchmark]?.ResultStatistics?.Mean ?? double.MaxValue
                    select benchmark;

                public string GetHighlightGroupKey(BenchmarkCase benchmarkCase) => benchmarkCase.DisplayInfo;

                public string GetLogicalGroupKey(ImmutableArray<BenchmarkCase> allBenchmarksCases, BenchmarkCase benchmarkCase) =>
                    benchmarkCase.Descriptor.DisplayInfo;

                public IEnumerable<IGrouping<string, BenchmarkCase>> GetLogicalGroupOrder(IEnumerable<IGrouping<string, BenchmarkCase>> logicalGroups) =>
                    logicalGroups.OrderBy(it => it.Key);

                public bool SeparateLogicalGroups => true;
            }
        }

        private const int StackSizeInBytes = 10000000;
        private const string TestPath = "Tests";

        private static readonly Dictionary<string, Test> Tests = new Dictionary<string, Test>();
        private static readonly string TestRunner = File.ReadAllText(Path.Combine(TestPath, "testrunner.js"));

        private static readonly Microsoft.ClearScript.V8.V8ScriptEngine clearscriptV8;
        private static readonly IronJS.Hosting.CSharp.Context ironjsEngine = new IronJS.Hosting.CSharp.Context();
        private static readonly Jurassic.ScriptEngine jurassicEngine = new Jurassic.ScriptEngine();
        private static readonly NiL.JS.Core.Context nilcontext = new NiL.JS.Core.Context();

        private static readonly Jint.Engine jintEngine = new Jint.Engine();
        private static readonly Esprima.ParserOptions jintParserOptions = new Esprima.ParserOptions
        {
            AdaptRegexp = true
        };

        private static readonly Dictionary<Engine, Action<bool, string>> engineRunners = new Dictionary<Engine, Action<bool, string>>
        {
            { Engine.ClearScriptV8, (reuseEngine, script) =>
                {
                    var context = reuseEngine
                        ? clearscriptV8
                        : new Microsoft.ClearScript.V8.V8ScriptEngine();
                    context.Execute(script);
                }
            },
            { Engine.IronJs, (reuseEngine, script) =>
                {
                    var engine = reuseEngine
                        ? ironjsEngine
                        : new IronJS.Hosting.CSharp.Context();
                    engine.Execute(script);
                }
            },
            { Engine.Jint, (reuseEngine, script) =>
                {
                    var engine = reuseEngine
                        ? jintEngine
                        : new Jint.Engine();
                    engine.Execute(script, jintParserOptions);
                }
            },
            { Engine.Jurassic, (reuseEngine, script) =>
                {
                    var engine = reuseEngine
                        ? jurassicEngine
                        : new Jurassic.ScriptEngine();
                    engine.Execute(script);
                }
            },
            { Engine.NilJs, (reuseEngine, script) =>
                {
                    var context = reuseEngine
                        ? nilcontext
                        : new NiL.JS.Core.Context();
                    context.Eval(script);
                }
            }
        };

        private Action<bool, string> scriptRunner;

        static EngineBenchmark()
        {
            try
            {
                clearscriptV8 = new Microsoft.ClearScript.V8.V8ScriptEngine();
            }
            catch
            {
                // will be reported as part of failures in reuse engine = false
            }
        }

        protected static void AddTest(int timeoutSeconds, string relativePath)
        {
            var fullName = Path.Combine(TestPath, relativePath);
            var fileInfo = new FileInfo(fullName);
            if (!fileInfo.Exists)
            {
                throw new FileNotFoundException(fullName);
            }

            var testContent = File.ReadAllText(fullName);
            var content = $"{TestRunner}{Environment.NewLine}{testContent}";

            Tests.Add(fileInfo.Name, new Test(fileInfo.Name, timeoutSeconds * 1000, content));
        }

        [Params(
            Engine.ClearScriptV8,
            Engine.IronJs,
            Engine.Jint,
            Engine.Jurassic,
            Engine.NilJs)]
        public Engine Engine { get; set; }

        [Params(true, false)]
        public bool ReuseEngine { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            scriptRunner = engineRunners[Engine];
        }

        protected void Run(string fileName)
        {
            var test = Tests[fileName];

            Exception error = null;
            var slim = new ManualResetEventSlim();
            var thread = new Thread(() =>
            {
                try
                {
                    scriptRunner(ReuseEngine, test.Content);
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

        private class Test
        {
            public Test(string name, int timeoutMs, string content)
            {
                Name = name;
                TimeoutMilliseconds = timeoutMs;
                Content = content;
            }

            public string Name { get; }
            public string Content { get; }
            public int TimeoutMilliseconds { get; }
        }
    }
}