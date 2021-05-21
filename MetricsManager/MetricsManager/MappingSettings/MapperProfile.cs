using System;
using AutoMapper;
using Core.Client.Generated;
using MetricsManager.DataAccessLayer.Models;
using MetricsManager.Responses;
using MetricsManager.Responses.DataTransferObjects;

namespace MetricsManager.MappingSettings
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            // добавлять сопоставления в таком стиле нужно для всех объектов 
            CreateMap<ApiCpuMetric, ApiCpuMetricDto>()
                .ForMember(dto => dto.Time, 
                    opt => opt.MapFrom(
                        src => DateTimeOffset.FromUnixTimeSeconds(src.Time.ToUnixTimeSeconds())));
            
            CreateMap<ApiDotNetMetric, ApiDotNetMetricDto>()
                .ForMember(dto => dto.Time, 
                    opt => opt.MapFrom(
                        src => DateTimeOffset.FromUnixTimeSeconds(src.Time.ToUnixTimeSeconds())));
            
            CreateMap<ApiHddMetric, ApiHddMetricDto>()
                .ForMember(dto => dto.Time, 
                    opt => opt.MapFrom(
                        src => DateTimeOffset.FromUnixTimeSeconds(src.Time.ToUnixTimeSeconds())));
            
            CreateMap<ApiNetworkMetric, ApiNetworkMetricDto>()
                .ForMember(dto => dto.Time, 
                    opt => opt.MapFrom(
                        src => DateTimeOffset.FromUnixTimeSeconds(src.Time.ToUnixTimeSeconds())));
            
            CreateMap<ApiRamMetric, ApiRamMetricDto>()
                .ForMember(dto => dto.Time, 
                    opt => opt.MapFrom(
                        src => DateTimeOffset.FromUnixTimeSeconds(src.Time.ToUnixTimeSeconds())));
            
            CreateMap<AgentInfo, AgentInfoDto>();

            CreateMap<CpuMetricDto, ApiCpuMetricDto>()
                .ForMember(dto => dto.Time, 
                    opt => opt.MapFrom(
                        src => DateTimeOffset.FromUnixTimeSeconds(src.Time.ToUnixTimeSeconds())));
            
            CreateMap<DotNetMetricDto, ApiDotNetMetricDto>()
                .ForMember(dto => dto.Time, 
                    opt => opt.MapFrom(
                        src => DateTimeOffset.FromUnixTimeSeconds(src.Time.ToUnixTimeSeconds())));
            
            CreateMap<HddMetricDto, ApiHddMetricDto>()
                .ForMember(dto => dto.Time, 
                    opt => opt.MapFrom(
                        src => DateTimeOffset.FromUnixTimeSeconds(src.Time.ToUnixTimeSeconds())));
            
            CreateMap<NetworkMetricDto, ApiNetworkMetricDto>()
                .ForMember(dto => dto.Time, 
                    opt => opt.MapFrom(
                        src => DateTimeOffset.FromUnixTimeSeconds(src.Time.ToUnixTimeSeconds())));
            
            CreateMap<RamMetricDto, ApiRamMetricDto>()
                .ForMember(dto => dto.Time, 
                    opt => opt.MapFrom(
                        src => DateTimeOffset.FromUnixTimeSeconds(src.Time.ToUnixTimeSeconds())));

            CreateMap<GetByPeriodCpuMetricsResponse, GetByPeriodCpuMetricsApiResponse>();
            CreateMap<GetByPeriodDotNetMetricsResponse, GetByPeriodDotNetMetricsApiResponse>();
            CreateMap<GetByPeriodHddMetricsResponse, GetByPeriodHddMetricsApiResponse>();
            CreateMap<GetByPeriodNetworkMetricsResponse, GetByPeriodNetworkMetricsApiResponse>();
            CreateMap<GetByPeriodRamMetricsResponse, GetByPeriodRamMetricsApiResponse>();
        }
    }

}