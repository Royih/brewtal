using System.Collections.Generic;
using Brewtal2.Brews;
using Brewtal2.Brews.Models;
using MediatR;

namespace Brewtal2.Infrastructure.Queries
{

    public class ListBrewsQuery : IRequest<IEnumerable<Brew>> { }

    public class ListBrewsQueryHandler : RequestHandler<ListBrewsQuery, IEnumerable<Brew>>
    {
        private readonly IBrewRepository _repo;

        public ListBrewsQueryHandler(IBrewRepository repo)
        {
            _repo = repo;
        }

        protected override IEnumerable<Brew> Handle(ListBrewsQuery query)
        {
            var t = _repo.ListBrews();
            return t;
        }
    }
}