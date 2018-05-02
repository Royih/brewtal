using System.Collections.Generic;
using AutoMapper;
using Brewtal.Database;
using Brewtal.Dtos;
using Brewtal.Extensions;

namespace Brewtal.CQRS
{
    public class AutoMapperProfileConfiguration : Profile
    {

        public AutoMapperProfileConfiguration()
        {
            CreateMap<Brew, BrewDto>()
                .ForMember(dest => dest.BeginMash, opt => opt.MapFrom(src => src.BeginMash.SpecifyUtcTime()))
                .ForMember(dest => dest.Initiated, opt => opt.MapFrom(src => src.Initiated.SpecifyUtcTime()));
            CreateMap<BrewStep, StepDto>()
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime.SpecifyUtcTime()))
                .ForMember(dest => dest.CompleteTime, opt => opt.MapFrom(src => src.CompleteTime.SpecifyUtcTime()));
        }

    }
}