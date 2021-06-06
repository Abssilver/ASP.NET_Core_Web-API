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
    public class AgentInfoClient: IAgentInfoClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<IAgentInfoClient> _logger;
        private readonly IMapper _mapper;
        private readonly IAppModel _appModel;

        public AgentInfoClient(
            HttpClient httpClient,
            ILogger<IAgentInfoClient> logger,
            IMapper mapper,
            IAppModel model)
        {
            _httpClient = httpClient;
            _logger = logger;
            _mapper = mapper;
            _appModel = model;
        }

        public void RegisterAgent(AgentInfoRegisterRequest request)
        {
            try
            {
                var generatedClient = new global::Client.GeneratedManager.Client(
                    _appModel.ManagerBaseAddress, _httpClient);
                generatedClient.ApiAgentsRegister(
                    new global::Client.GeneratedManager.AgentInfoRegisterRequest
                    {
                        Address = request.Address
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public void UnregisterAgent(AgentInfoUnregisterRequest request)
        {
            try
            {
                var generatedClient = new global::Client.GeneratedManager.Client(
                    _appModel.ManagerBaseAddress, _httpClient);
                generatedClient.ApiAgentsUnregister(
                    new global::Client.GeneratedManager.AgentInfoUnregisterRequest
                    {
                        Address = request.Address
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public GetAgentsInfoClientResponse GetRegisterAgents()
        {
            try
            {
                var generatedClient = new global::Client.GeneratedManager.Client(
                    _appModel.ManagerBaseAddress, _httpClient);
                var response =  generatedClient.ApiAgentsGetAgents();
                var apiResponse = _mapper.Map<GetAgentsInfoClientResponse>(response);
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