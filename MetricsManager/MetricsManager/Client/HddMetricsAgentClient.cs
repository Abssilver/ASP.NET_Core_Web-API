using System;
using System.Net.Http;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public HddMetricsAgentClient(
            HttpClient httpClient, 
            ILogger<IHddMetricsAgentClient> logger,
            IMapper mapper)
        {
            _httpClient = httpClient;
            _logger = logger;
            _mapper = mapper;
        }

        public GetByPeriodHddMetricsApiResponse GetHddMetrics(HddMetricsApiGetRequest request)
        {
            try
            {
                var generatedClient = new Core.Client.Generated.Client(request.ClientBaseAddress, _httpClient);
                var response =  generatedClient.ApiMetricsHddFromTo(request.FromTime, request.ToTime);
                var apiResponse = _mapper.Map<GetByPeriodHddMetricsApiResponse>(response);
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