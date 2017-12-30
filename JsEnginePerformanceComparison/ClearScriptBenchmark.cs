using System;

namespace JsEnginePerformanceComparison
{
    public class ClearScriptBenchmark : EngineBenchmark
    {
        private static readonly Microsoft.ClearScript.V8.V8ScriptEngine clearscriptV8;

        static ClearScriptBenchmark()
        {
            try
            {
                clearscriptV8 = new Microsoft.ClearScript.V8.V8ScriptEngine();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("V8ScriptEngine load failed, tests fill fail: " + ex.Message);
            }
        }

        protected override void RunScript(string script)
        {
            clearscriptV8.Execute(script);
        }
    }
}