using System;
using System.Net.Http;
using AutoMapper;
using MetricsManager.Client.Interfaces;
using MetricsManager.Requests;
using MetricsManager.Responses;
using Microsoft.Extensions.Logging;

namespace MetricsManager.Client
{
    public class CpuMetricsAgentClient : ICpuMetricsAgentClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ICpuMetricsAgentClient> _logger;
        private readonly IMapper _mapper;

        public CpuMetricsAgentClient(
            HttpClient httpClient,
            ILogger<ICpuMetricsAgentClient> logger,
            IMapper mapper)
        {
            _httpClient = httpClient;
            _logger = logger;
            _mapper = mapper;
        }

        public GetByPeriodCpuMetricsApiResponse GetCpuMetrics(CpuMetricApiGetRequest request)
        {
            try
            {
                var generatedClient = new Core.Client.Generated.Client(request.ClientBaseAddress, _httpClient);
                var response =  generatedClient.ApiMetricsCpuFromTo(request.FromTime, request.ToTime);
                var apiResponse = _mapper.Map<GetByPeriodCpuMetricsApiResponse>(response);
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