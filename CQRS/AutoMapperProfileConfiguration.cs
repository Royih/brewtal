using System.Collections.Generic;
using AutoMapper;
using Brewtal.Database;
using Brewtal.Dtos;

namespace Brewtal.CQRS
{
    public class AutoMapperProfileConfiguration : Profile
    {

        public AutoMapperProfileConfiguration()
        {
            CreateMap<Brew, BrewDto>();
        }

    }
}