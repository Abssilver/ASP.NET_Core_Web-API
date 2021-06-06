using System;
using AutoMapper;
using Client.GeneratedManager;
using MetricsManagerClient.Responses;
using MetricsManagerClient.Responses.DataTransferObjects;

namespace MetricsManagerClient.MappingSettings
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<ApiCpuMetricDto, CpuMetricClientDto>()
                .ForMember(dto => dto.Time, 
                    opt => opt.MapFrom(
                        src => DateTimeOffset.FromUnixTimeSeconds(src.Time.ToUnixTimeSeconds())));
            
            CreateMap<ApiDotNetMetricDto, DotNetMetricClientDto>()
                .ForMember(dto => dto.Time, 
                    opt => opt.MapFrom(
                        src => DateTimeOffset.FromUnixTimeSeconds(src.Time.ToUnixTimeSeconds())));
            
            CreateMap<ApiHddMetricDto, HddMetricClientDto>()
                .ForMember(dto => dto.Time, 
                    opt => opt.MapFrom(
                        src => DateTimeOffset.FromUnixTimeSeconds(src.Time.ToUnixTimeSeconds())));
            
            CreateMap<ApiNetworkMetricDto, NetworkMetricClientDto>()
                .ForMember(dto => dto.Time, 
                    opt => opt.MapFrom(
                        src => DateTimeOffset.FromUnixTimeSeconds(src.Time.ToUnixTimeSeconds())));
            
            CreateMap<ApiRamMetricDto, RamMetricClientDto>()
                .ForMember(dto => dto.Time, 
                    opt => opt.MapFrom(
                        src => DateTimeOffset.FromUnixTimeSeconds(src.Time.ToUnixTimeSeconds())));
            
            CreateMap<AgentInfoDto, AgentInfoClientDto>();
            
            CreateMap<GetByPeriodCpuMetricsApiResponse, GetByPeriodCpuMetricsClientResponse>();
            CreateMap<GetByPeriodDotNetMetricsApiResponse, GetByPeriodDotNetMetricsClientResponse>();
            CreateMap<GetByPeriodHddMetricsApiResponse, GetByPeriodHddMetricsClientResponse>();
            CreateMap<GetByPeriodNetworkMetricsApiResponse, GetByPeriodNetworkMetricsClientResponse>();
            CreateMap<GetByPeriodRamMetricsApiResponse, GetByPeriodRamMetricsClientResponse>();
        }
    }

}