using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MetricsManager.Client.Interfaces;
using MetricsManager.Requests;
using MetricsManager.Responses;
using Microsoft.Extensions.Logging;

namespace MetricsManager.Client
{
    public class MetricsAgentClient : Core.Client.Generated.Client, IMetricsAgentClient
    {
        private readonly ILogger<ICpuMetricsAgentClient> _logger;
        public MetricsAgentClient(HttpClient httpClient, ILogger<IMetricsAgentClient> logger) : 
            base(string.Empty, httpClient)
        {
            _logger = logger;
        }

        public GetByPeriodCpuMetricsApiResponse GetCpuMetrics(CpuMetricApiGetRequest request)
        {
            return GetCpuMetricsAsync(request).Result;
        }

        private async Task<GetByPeriodCpuMetricsApiResponse> GetCpuMetricsAsync(CpuMetricApiGetRequest request)
        {
            try
            {
                BaseUrl = request.ClientBaseAddress;
                await ApiMetricsCpuFromToAsync(request.FromTime, request.ToTime);
                var response = await ReadObjectResponseAsync<GetByPeriodCpuMetricsApiResponse>(
                    base.ResponseMessage, null, CancellationToken.None);
                return response.Object;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }

        public GetByPeriodDotNetMetricsApiResponse GetDotNetMetrics(DotNetMetricApiGetRequest request)
        {
            try
            {
                BaseUrl = request.ClientBaseAddress;
                ApiMetricsDotnetFromTo(request.FromTime, request.ToTime);
                var response = ReadObjectResponseAsync<GetByPeriodDotNetMetricsApiResponse>(
                    base.ResponseMessage, null, CancellationToken.None);
                return response.Result.Object;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }

        public GetByPeriodHddMetricsApiResponse GetHddMetrics(HddMetricsApiGetRequest request)
        {
            try
            {
                BaseUrl = request.ClientBaseAddress;
                ApiMetricsHddFromTo(request.FromTime, request.ToTime);
                var response = ReadObjectResponseAsync<GetByPeriodHddMetricsApiResponse>(
                    base.ResponseMessage, null, CancellationToken.None);
                return response.Result.Object;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }

        public GetByPeriodNetworkMetricsApiResponse GetNetworkMetrics(NetworkMetricApiGetRequest request)
        {
            try
            {
                BaseUrl = request.ClientBaseAddress;
                ApiMetricsCpuFromTo(request.FromTime, request.ToTime);
                var response = ReadObjectResponseAsync<GetByPeriodNetworkMetricsApiResponse>(
                    base.ResponseMessage, null, CancellationToken.None);
                return response.Result.Object;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }

        public GetByPeriodRamMetricsApiResponse GetRamMetrics(RamMetricApiGetRequest request)
        {
            try
            {
                BaseUrl = request.ClientBaseAddress;
                ApiMetricsCpuFromTo(request.FromTime, request.ToTime);
                var response = ReadObjectResponseAsync<GetByPeriodRamMetricsApiResponse>(
                    base.ResponseMessage, null, CancellationToken.None);
                return response.Result.Object;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }
    }
}