using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Brewtal2.Storage;
using MediatR;

namespace Brewtal2.Pid.Commands
{
    public class GetSessionQuery : IRequest<SessionDto>
    {
        public int SessionId { get; set; } = 0;
    }

    public class GetSessionQueryHandler : IRequestHandler<GetSessionQuery, SessionDto>
    {
        private readonly IStorageRepository _storageRepo;
        private readonly IMapper _mapper;

        public GetSessionQueryHandler(IStorageRepository storageRepository, IMapper mapper)
        {
            _storageRepo = storageRepository;
            _mapper = mapper;
        }

        public async Task<SessionDto> Handle(GetSessionQuery query, CancellationToken cancellationToken)
        {
            var data = _mapper.Map<SessionDto>(await _storageRepo.GetSessionAsync(query.SessionId));
            var allSessions = await _storageRepo.ListSessions();
            data.AllSessions = _mapper.Map<List<SessionLightDto>>(allSessions);
            return data;
        }

    }

}