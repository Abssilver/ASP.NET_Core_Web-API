using System;
using System.Net.Http;
using System.Text.Json;
using MetricsManager.Client.Interfaces;
using MetricsManager.Requests;
using MetricsManager.Responses;
using Microsoft.Extensions.Logging;

namespace MetricsManager.Client
{
    public class DotNetMetricsAgentClient : IDotNetMetricsAgentClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<IDotNetMetricsAgentClient> _logger;
        
        public DotNetMetricsAgentClient(HttpClient httpClient, ILogger<IDotNetMetricsAgentClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public GetByPeriodDotNetMetricsApiResponse GetDotNetMetrics(DotNetMetricApiGetRequest request)
        {
            var dateFormat = "yyyy-MM-ddTHH:mm:ss";
            var fromParameter = request.FromTime.ToString(dateFormat);
            var toParameter = request.ToTime.ToString(dateFormat);
            var httpRequest = new HttpRequestMessage(
                HttpMethod.Get,
                $"{request.ClientBaseAddress}/api/metrics/dotnet/from/{fromParameter}/to/{toParameter}");
            try
            {
                HttpResponseMessage response = _httpClient.SendAsync(httpRequest).Result;

                using var responseStream = response.Content.ReadAsStreamAsync().Result;
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };
                return JsonSerializer.DeserializeAsync<GetByPeriodDotNetMetricsApiResponse>(responseStream, options).Result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }
    }
}