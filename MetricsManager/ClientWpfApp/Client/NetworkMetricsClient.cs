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
    public class NetworkMetricsClient : INetworkMetricsClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<INetworkMetricsClient> _logger;
        private readonly IMapper _mapper;
        private readonly IAppModel _appModel;

    
        public NetworkMetricsClient(
            HttpClient httpClient, 
            ILogger<INetworkMetricsClient> logger,
            IMapper mapper,
            IAppModel model)
        {
            _httpClient = httpClient;
            _logger = logger;
            _mapper = mapper;
            _appModel = model;
        }

        public GetByPeriodNetworkMetricsClientResponse GetMetricsFromAgent(GetNetworkMetricsFromAgentRequest request)
        {
            try
            {
                var generatedClient = new global::Client.GeneratedManager.Client(
                    _appModel.ManagerBaseAddress, _httpClient);
                var response =  generatedClient.ApiMetricsNetworkAgentFromTo(
                    request.AgentId, request.FromTime, request.ToTime);
                var apiResponse = _mapper.Map<GetByPeriodNetworkMetricsClientResponse>(response);
                return apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }
        
        public GetByPeriodNetworkMetricsClientResponse GetMetricsFromAllCluster(GetAllNetworkMetricsRequest request)
        {
            try
            {
                var generatedClient = new global::Client.GeneratedManager.Client(
                    _appModel.ManagerBaseAddress, _httpClient);
                var response =  generatedClient.ApiMetricsNetworkClusterFromTo(
                    request.FromTime, request.ToTime);
                var apiResponse = _mapper.Map<GetByPeriodNetworkMetricsClientResponse>(response);
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