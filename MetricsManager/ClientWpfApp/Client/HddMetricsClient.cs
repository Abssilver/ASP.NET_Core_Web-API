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
    public class HddMetricsClient : IHddMetricsClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<IHddMetricsClient> _logger;
        private readonly IMapper _mapper;
        private readonly IAppModel _appModel;


        public HddMetricsClient(
            HttpClient httpClient, 
            ILogger<IHddMetricsClient> logger,
            IMapper mapper,
            IAppModel model)
        {
            _httpClient = httpClient;
            _logger = logger;
            _mapper = mapper;
            _appModel = model;
        }

        public GetByPeriodHddMetricsClientResponse GetMetricsFromAgent(GetHddMetricsFromAgentRequest request)
        {
            try
            {
                var generatedClient = new global::Client.GeneratedManager.Client(
                    _appModel.ManagerBaseAddress, _httpClient);
                var response =  generatedClient.ApiMetricsHddAgentFromTo(
                    request.AgentId, request.FromTime, request.ToTime);
                var apiResponse = _mapper.Map<GetByPeriodHddMetricsClientResponse>(response);
                return apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }

        public GetByPeriodHddMetricsClientResponse GetMetricsFromAllCluster(GetAllHddMetricsRequest request)
        {
            try
            {
                var generatedClient = new global::Client.GeneratedManager.Client(
                    _appModel.ManagerBaseAddress, _httpClient);
                var response =  generatedClient.ApiMetricsHddClusterFromTo(
                    request.FromTime, request.ToTime);
                var apiResponse = _mapper.Map<GetByPeriodHddMetricsClientResponse>(response);
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