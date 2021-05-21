using System;
using System.Net.Http;
using AutoMapper;
using MetricsManager.Client.Interfaces;
using MetricsManager.Requests;
using MetricsManager.Responses;
using Microsoft.Extensions.Logging;

namespace MetricsManager.Client
{
    public class NetworkMetricsAgentClient : INetworkMetricsAgentClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<INetworkMetricsAgentClient> _logger;
        private readonly IMapper _mapper;

        public NetworkMetricsAgentClient(
            HttpClient httpClient, 
            ILogger<INetworkMetricsAgentClient> logger,
            IMapper mapper)
        {
            _httpClient = httpClient;
            _logger = logger;
            _mapper = mapper;
        }

        public GetByPeriodNetworkMetricsApiResponse GetNetworkMetrics(NetworkMetricApiGetRequest request)
        {
            try
            {
                var generatedClient = new Core.Client.Generated.Client(request.ClientBaseAddress, _httpClient);
                var response =  generatedClient.ApiMetricsNetworkFromTo(request.FromTime, request.ToTime);
                var apiResponse = _mapper.Map<GetByPeriodNetworkMetricsApiResponse>(response);
                return apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }
    }
}