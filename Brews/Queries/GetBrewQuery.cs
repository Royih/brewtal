using System;
using Brewtal2.Brews;
using Brewtal2.Brews.Models;
using MediatR;

namespace Brewtal2.Infrastructure.Queries
{

    public class GetBrewQuery : IRequest<Brew>
    {
        public Guid Id { get; set; }
    }

    public class GetBrewQueryHandler : RequestHandler<GetBrewQuery, Brew>
    {
        private readonly IBrewRepository _repo;

        public GetBrewQueryHandler(IBrewRepository repo)
        {
            _repo = repo;
        }

        protected override Brew Handle(GetBrewQuery query)
        {
            var t = _repo.GetBrew(query.Id.ToString());
            return t;
        }
    }
}