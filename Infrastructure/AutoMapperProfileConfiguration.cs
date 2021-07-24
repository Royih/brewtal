using System.Linq;
using AutoMapper;
using Brewtal2.Storage;

namespace Brewtal2.Infrastructure
{
    public class AutoMapperProfileConfiguration : Profile
    {

        public AutoMapperProfileConfiguration()
        {
            CreateMap<Session, SessionDto>().ForMember(dest => dest.Logs, opt => opt.MapFrom(src => src.Logs.OrderBy(x => x.Id)));
            CreateMap<Session, SessionLightDto>();
            CreateMap<Runtime, RuntimeDto>();
            CreateMap<Templog, TemplogDto>();

        }

    }
}