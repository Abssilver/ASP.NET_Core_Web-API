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
    public class DotNetMetricsClient : IDotNetMetricsClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<IDotNetMetricsClient> _logger;
        private readonly IMapper _mapper;
        private readonly IAppModel _appModel;


        public DotNetMetricsClient(
            HttpClient httpClient, 
            ILogger<IDotNetMetricsClient> logger,
            IMapper mapper,
            IAppModel model)
        {
            _httpClient = httpClient;
            _logger = logger;
            _mapper = mapper;
            _appModel = model;
        }

        public GetByPeriodDotNetMetricsClientResponse GetMetricsFromAgent(GetDotNetMetricsFromAgentRequest request)
        {
            try
            {
                var generatedClient = new global::Client.GeneratedManager.Client(
                    _appModel.ManagerBaseAddress, _httpClient);
                var response =  generatedClient.ApiMetricsDotnetAgentFromTo(
                    request.AgentId, request.FromTime, request.ToTime);
                var apiResponse = _mapper.Map<GetByPeriodDotNetMetricsClientResponse>(response);
                return apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }

        public GetByPeriodDotNetMetricsClientResponse GetMetricsFromAllCluster(GetAllDotNetMetricsRequest request)
        {
            try
            {
                var generatedClient = new global::Client.GeneratedManager.Client(
                    _appModel.ManagerBaseAddress, _httpClient);
                var response =  generatedClient.ApiMetricsDotnetClusterFromTo(
                    request.FromTime, request.ToTime);
                var apiResponse = _mapper.Map<GetByPeriodDotNetMetricsClientResponse>(response);
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