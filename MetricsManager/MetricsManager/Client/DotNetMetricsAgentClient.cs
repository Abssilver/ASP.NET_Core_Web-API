using System;
using System.Net.Http;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public DotNetMetricsAgentClient(
            HttpClient httpClient, 
            ILogger<IDotNetMetricsAgentClient> logger,
            IMapper mapper)
        {
            _httpClient = httpClient;
            _logger = logger;
            _mapper = mapper;
        }

        public GetByPeriodDotNetMetricsApiResponse GetDotNetMetrics(DotNetMetricApiGetRequest request)
        {
            try
            {
                var generatedClient = new Core.Client.Generated.Client(request.ClientBaseAddress, _httpClient);
                var response =  generatedClient.ApiMetricsDotnetFromTo(request.FromTime, request.ToTime);
                var apiResponse = _mapper.Map<GetByPeriodDotNetMetricsApiResponse>(response);
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