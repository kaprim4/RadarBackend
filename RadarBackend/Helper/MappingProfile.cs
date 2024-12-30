using AutoMapper;
using Domain.DTOs;
using Domain.Models;
using Domain.Models.Data;

namespace RadarBackend.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Simplify LotDTO <-> Lot mapping
            CreateMap<LotDTO, Lot>()
                .ReverseMap();

            // DocumentDTO <-> Document mapping
            CreateMap<DocumentDTO, Document>()
                .ForMember(dest => dest.Jmx, opt => opt.MapFrom(src => src.Jmx))
                .ForMember(dest => dest.Lot, opt => opt.Ignore())
                .ReverseMap();

            // JMXDTO <-> JMX mapping
            CreateMap<JMXDTO, JMX>()
                .ForMember(dest => dest.VehicleDatas, opt => opt.MapFrom(src => src.VehicleDatas))
                .ForMember(dest => dest.DeploymentSummary, opt => opt.MapFrom(src => src.DeploymentSummary))
                .ForMember(dest => dest.Document, opt => opt.Ignore())
                .ReverseMap();

            // VehicleDataDTO <-> VehicleData mapping
            CreateMap<VehicleDataDTO, VehicleData>()
                .ForMember(dest => dest.JMX, opt => opt.Ignore())
                .ReverseMap();

            // DeploymentSummaryDTO <-> DeploymentSummary mapping
            CreateMap<DeploymentSummaryDTO, DeploymentSummary>()
                .ForMember(dest => dest.JMX, opt => opt.Ignore())
                .ReverseMap();

        }

    }
}
