using System;
using AutoMapper;
using MetricsManager.DataAccessLayer.Models;
using MetricsManager.Responses.DataTransferObjects;

namespace MetricsManager.MappingSettings
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            // добавлять сопоставления в таком стиле нужно для всех объектов 
            CreateMap<CpuMetric, CpuMetricDto>()
                .ForMember(dto => dto.Time, 
                    opt => opt.MapFrom(
                        src => DateTimeOffset.FromUnixTimeSeconds(src.Time.ToUnixTimeSeconds())));
            
            CreateMap<DotNetMetric, DotNetMetricDto>()
                .ForMember(dto => dto.Time, 
                    opt => opt.MapFrom(
                        src => DateTimeOffset.FromUnixTimeSeconds(src.Time.ToUnixTimeSeconds())));
            
            CreateMap<HddMetric, HddMetricDto>()
                .ForMember(dto => dto.Time, 
                    opt => opt.MapFrom(
                        src => DateTimeOffset.FromUnixTimeSeconds(src.Time.ToUnixTimeSeconds())));
            
            CreateMap<NetworkMetric, NetworkMetricDto>()
                .ForMember(dto => dto.Time, 
                    opt => opt.MapFrom(
                        src => DateTimeOffset.FromUnixTimeSeconds(src.Time.ToUnixTimeSeconds())));
            
            CreateMap<RamMetric, RamMetricDto>()
                .ForMember(dto => dto.Time, 
                    opt => opt.MapFrom(
                        src => DateTimeOffset.FromUnixTimeSeconds(src.Time.ToUnixTimeSeconds())));
            
            CreateMap<AgentInfo, AgentInfoDto>();
        }
    }

}