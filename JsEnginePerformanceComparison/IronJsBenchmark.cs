namespace JsEnginePerformanceComparison
{
    public class IronJsBenchmark : EngineBenchmark
    {
        private static readonly IronJS.Hosting.CSharp.Context ironjsEngine = new IronJS.Hosting.CSharp.Context();

        protected override void RunScript(string script)
        {
            ironjsEngine.Execute(script);
        }
    }
}