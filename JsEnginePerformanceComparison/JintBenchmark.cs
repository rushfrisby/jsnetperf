using Esprima;

namespace JsEnginePerformanceComparison
{
    public class JintBenchmark : EngineBenchmark
    {
        private static readonly Jint.Engine jintEngine = new Jint.Engine();

        private static readonly ParserOptions parserOptions = new ParserOptions
        {
            AdaptRegexp = true,
            Tolerant = true,
            Loc = false
        };

        protected override void RunScript(string script)
        {
            jintEngine.Execute(script, parserOptions);
        }
    }
}