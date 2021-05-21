using System.Net.Http;

namespace MetricsManager.Client
{
    public class MetricsAgentClient : Core.Client.Generated.Client
    {
        public MetricsAgentClient(HttpClient httpClient) : base(string.Empty, httpClient)
        { }
    }
}