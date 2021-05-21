using System;
using System.Net.Http;
using AutoMapper;
using MetricsManager.Client.Interfaces;
using MetricsManager.Requests;
using MetricsManager.Responses;
using Microsoft.Extensions.Logging;

namespace MetricsManager.Client
{
    public class RamMetricsAgentClient : IRamMetricsAgentClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<IRamMetricsAgentClient> _logger;
        private readonly IMapper _mapper;

        public RamMetricsAgentClient(
            HttpClient httpClient,
            ILogger<IRamMetricsAgentClient> logger,
            IMapper mapper)
        {
            _httpClient = httpClient;
            _logger = logger;
            _mapper = mapper;
        }

        public GetByPeriodRamMetricsApiResponse GetRamMetrics(RamMetricApiGetRequest request)
        {
            try
            {
                var generatedClient = new Core.Client.Generated.Client(request.ClientBaseAddress, _httpClient);
                var response =  generatedClient.ApiMetricsRamFromTo(request.FromTime, request.ToTime);
                var apiResponse = _mapper.Map<GetByPeriodRamMetricsApiResponse>(response);
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