namespace JsEnginePerformanceComparison
{
    public class NilJsBenchmark : EngineBenchmark
    {
        private static readonly NiL.JS.Core.Context nilcontext = new NiL.JS.Core.Context();

        protected override void RunScript(string script)
        {
            nilcontext.Eval(script);
        }
    }
}