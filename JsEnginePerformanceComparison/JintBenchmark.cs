namespace JsEnginePerformanceComparison
{
    public class JintBenchmark : EngineBenchmark
    {
        private static readonly Jint.Engine jintEngine = new Jint.Engine();

        protected override void RunScript(string script)
        {
            jintEngine.Execute(script);
        }
    }
}