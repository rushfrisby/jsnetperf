using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using ThreadState = System.Threading.ThreadState;

namespace JsEnginePerformanceComparison
{
    class Program
    {
        private const int StackSizeInBytes = 10000000;
        private static readonly List<Test> Tests = new List<Test>();
        private static readonly string TestPath = Path.Combine(AssemblyDirectory, "Tests");
        private static readonly string Testrunner = File.ReadAllText(Path.Combine(TestPath, "testrunner.js"));

        static void Main()
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

            foreach (var test in Tests)
            {
                ExecuteWithJint(test);
                ExecuteWithIronJs(test);
                ExecuteWithJavascriptDotNet(test);
                ExecuteWithJurrasic(test);
                ExecuteWithClearScript(test);
                ExecuteWithNilJs(test);
            }

            Console.WriteLine("DONE");
            Console.Read();
        }

        static void AddTest(int timeoutSeconds, string relativePath)
        {
            var fullName = Path.Combine(TestPath, relativePath);
            var fileInfo = new FileInfo(fullName);
            if(!fileInfo.Exists)
                throw new FileNotFoundException(fullName);

            var testContent = File.ReadAllText(fullName);
            var content = String.Format("{0}{1}{2}", Testrunner, Environment.NewLine, testContent);

            Tests.Add(new Test(fileInfo.Name, timeoutSeconds * 1000, content));
        }

        private static void ExecuteWithNilJs(Test test)
        {
            Execute("niljs", test, () =>
            {
                var nilcontext = new NiL.JS.Core.Context();
                nilcontext.Eval(test.Content);
            });
        }

        private static void ExecuteWithClearScript(Test test)
        {
            Execute("clearscript", test, () =>
            {
                using (var clearscriptV8 = new Microsoft.ClearScript.V8.V8ScriptEngine())
                {
                    clearscriptV8.Execute(test.Content);
                }
            });
        }

        private static void ExecuteWithJurrasic(Test test)
        {
            Execute("jurassic", test, () =>
            {
                var jurassicEngine = new Jurassic.ScriptEngine();
                jurassicEngine.Execute(test.Content);
            });
        }

        private static void ExecuteWithJint(Test test)
        {
            Execute("jint", test, () =>
            {
                var jintEngine = new Jint.Engine();
                jintEngine.Execute(test.Content);
            });
        }

        private static void ExecuteWithIronJs(Test test)
        {
            Execute("ironjs", test, () =>
            {
                var ironjsEngine = new IronJS.Hosting.CSharp.Context();
                ironjsEngine.Execute(test.Content);
            });
        }

        private static void ExecuteWithJavascriptDotNet(Test test)
        {
            Execute("js.net", test, () =>
            {
                using (var jsNetEngine = new Noesis.Javascript.JavascriptContext())
                {
                    jsNetEngine.Run(test.Content);
                }
            });
        }

        private static void Execute(string engineName, Test test, Action action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var timeout = false;
            var error = false;
            var sw = new Stopwatch();
            var thread = new Thread(() =>
            {
                try
                {
                    action();
                }
                catch (TimeoutException)
                {
                    timeout = true;
                }
                catch(Exception ex)
                {
                    error = true;
                }
            }, StackSizeInBytes);

            thread.Start();
            sw.Start();

            while (true)
            {
                if (sw.ElapsedMilliseconds > test.TimeoutMilliseconds)
                {
                    thread.Abort();
                    timeout = true;
                    break;
                }

                if (thread.ThreadState == ThreadState.Stopped)
                    break;

                Thread.Sleep(1);
            }

            sw.Stop();

            Console.WriteLine(test.Name + " - " + engineName + ": " + sw.ElapsedMilliseconds + (error ? " *** ERROR ***" : "") + (timeout ? " *** TIMEOUT ***" : ""));
        }

        public static string AssemblyDirectory
        {
            get
            {
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
    }
}