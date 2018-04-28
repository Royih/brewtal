using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Brewtal.Database;
using Brewtal.Dtos;
using MediatR;

namespace Brewtal.CQRS
{

    public class ListDataCaptureValueDefinitionsQuery : IRequest<IEnumerable<DataCaptureValueDto>>
    {
        public int BrewId { get; set; }
    }

    public class ListDataCaptureValueDefinitionsQueryHandler : RequestHandler<ListDataCaptureValueDefinitionsQuery, IEnumerable<DataCaptureValueDto>>
    {
        private readonly BrewtalContext _db;

        public ListDataCaptureValueDefinitionsQueryHandler(BrewtalContext db)
        {
            _db = db;
        }

        protected override IEnumerable<DataCaptureValueDto> HandleCore(ListDataCaptureValueDefinitionsQuery query)
        {
            //todo: improve this. Warnings..
            var v1 = _db.DataCaptureFloatValues.Where(x => x.BrewStep.BrewId == query.BrewId && x.Value.HasValue).Select(x => new DataCaptureValueDto
            {
                Id = x.Id,
                BrewStepId = x.BrewStepId,
                Label = x.Label,
                ValueAsString = x.Value.ToString(),
                ValueType = "float",
                Units = x.Units,
                Optional = x.Optional
            });
            var v2 = _db.DataCaptureIntValues.Where(x => x.BrewStep.BrewId == query.BrewId && x.Value.HasValue).Select(x => new DataCaptureValueDto
            {
                Id = x.Id,
                BrewStepId = x.BrewStepId,
                Label = x.Label,
                ValueAsString = x.Value.ToString(),
                ValueType = "int",
                Units = x.Units,
                Optional = x.Optional
            });
            var v3 = _db.DataCaptureStringValues.Where(x => x.BrewStep.BrewId == query.BrewId && x.Value != null && x.Value != "").Select(x => new DataCaptureValueDto
            {
                Id = x.Id,
                BrewStepId = x.BrewStepId,
                Label = x.Label,
                ValueAsString = x.Value,
                ValueType = "string",
                Units = x.Units,
                Optional = x.Optional
            });
            var r = v1.Union(v2).Union(v3).OrderBy(x => x.BrewStepId).ThenBy(x => x.Label).ToArray();
            return r.AsEnumerable();
        }
    }
}
