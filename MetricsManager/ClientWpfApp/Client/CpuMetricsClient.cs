using System;
using System.Net.Http;
using AutoMapper;
using MetricsManagerClient.Client.Interfaces;
using MetricsManagerClient.DataLayer.Interfaces;
using MetricsManagerClient.Requests;
using MetricsManagerClient.Responses;
using Microsoft.Extensions.Logging;

namespace MetricsManagerClient.Client
{
    public class CpuMetricsClient : ICpuMetricsClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ICpuMetricsClient> _logger;
        private readonly IMapper _mapper;
        private readonly IAppModel _appModel;


        public CpuMetricsClient(
            HttpClient httpClient,
            ILogger<ICpuMetricsClient> logger,
            IMapper mapper,
            IAppModel model)
        {
            _httpClient = httpClient;
            _logger = logger;
            _mapper = mapper;
            _appModel = model;
        }

        public GetByPeriodCpuMetricsClientResponse GetMetricsFromAgent(GetCpuMetricsFromAgentRequest request)
        {
            try
            {
                var generatedClient = new global::Client.GeneratedManager.Client(
                    _appModel.ManagerBaseAddress, _httpClient);
                var response =  generatedClient.ApiMetricsCpuAgentFromTo(
                    request.AgentId, request.FromTime, request.ToTime);
                var apiResponse = _mapper.Map<GetByPeriodCpuMetricsClientResponse>(response);
                return apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }

        public GetByPeriodCpuMetricsClientResponse GetMetricsFromAllCluster(GetAllCpuMetricsRequest request)
        {
            try
            {
                var generatedClient = new global::Client.GeneratedManager.Client(
                    _appModel.ManagerBaseAddress, _httpClient);
                var response =  generatedClient.ApiMetricsCpuClusterFromTo(
                    request.FromTime, request.ToTime);
                var apiResponse = _mapper.Map<GetByPeriodCpuMetricsClientResponse>(response);
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