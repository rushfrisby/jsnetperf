
namespace JsEnginePerformanceComparison
{
    class Test
    {
        public Test(string name, int timeoutMs, string content)
        {
            Name = name;
            TimeoutMilliseconds = timeoutMs;
            Content = content;
        }

        public string Name { get; set; }
        public string Content { get; set; }
        public int TimeoutMilliseconds { get; set; }
    }
}
