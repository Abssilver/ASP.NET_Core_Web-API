using System;
using System.Net.Http;
using System.Text.Json;
using MetricsManager.Client.Interfaces;
using MetricsManager.Requests;
using MetricsManager.Responses;
using Microsoft.Extensions.Logging;

namespace MetricsManager.Client
{
    public class HddMetricsAgentClient : IHddMetricsAgentClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<IHddMetricsAgentClient> _logger;
        
        public HddMetricsAgentClient(HttpClient httpClient, ILogger<IHddMetricsAgentClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public GetByPeriodHddMetricsApiResponse GetHddMetrics(HddMetricsApiGetRequest request)
        {
            var dateFormat = "yyyy-MM-ddTHH:mm:ss";
            var fromParameter = request.FromTime.ToString(dateFormat);
            var toParameter = request.ToTime.ToString(dateFormat);
            var httpRequest = new HttpRequestMessage(
                HttpMethod.Get,
                $"{request.ClientBaseAddress}/api/metrics/hdd/from/{fromParameter}/to/{toParameter}");
            try
            {
                HttpResponseMessage response = _httpClient.SendAsync(httpRequest).Result;

                using var responseStream = response.Content.ReadAsStreamAsync().Result;
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };
                return JsonSerializer.DeserializeAsync<GetByPeriodHddMetricsApiResponse>(responseStream, options).Result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }
    }
}