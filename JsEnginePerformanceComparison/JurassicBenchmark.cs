namespace JsEnginePerformanceComparison
{
    public class JurassicBenchmark : EngineBenchmark
    {
        private static readonly Jurassic.ScriptEngine jurassicEngine = new Jurassic.ScriptEngine();

        protected override void RunScript(string script)
        {
            jurassicEngine.Execute(script);
        }
    }
}