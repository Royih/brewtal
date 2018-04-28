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

    public class ListDataCaptureValuesQuery : IRequest<IEnumerable<DataCaptureValueDto>>
    {
        public int BrewStepId { get; set; }
    }

    public class ListDataCaptureValuesQueryHandler : RequestHandler<ListDataCaptureValuesQuery, IEnumerable<DataCaptureValueDto>>
    {
        private readonly BrewtalContext _db;

        public ListDataCaptureValuesQueryHandler(BrewtalContext db)
        {
            _db = db;
        }
        protected override IEnumerable<DataCaptureValueDto> HandleCore(ListDataCaptureValuesQuery query)
        {
            var floatValues = _db.DataCaptureFloatValues.Where(x => x.BrewStepId == query.BrewStepId).Select(x => new DataCaptureValueDto
            {
                Id = x.Id,
                BrewStepId = query.BrewStepId,
                Label = x.Label,
                ValueAsString = x.Value.ToString(),
                ValueType = "float",
                Units = x.Units,
                Optional = x.Optional
            });
            var intValues = _db.DataCaptureIntValues.Where(x => x.BrewStepId == query.BrewStepId).Select(x => new DataCaptureValueDto
            {
                Id = x.Id,
                BrewStepId = query.BrewStepId,
                Label = x.Label,
                ValueAsString = x.Value.ToString(),
                ValueType = "int",
                Units = x.Units,
                Optional = x.Optional
            });
            var stringValues = _db.DataCaptureStringValues.Where(x => x.BrewStepId == query.BrewStepId).Select(x => new DataCaptureValueDto
            {
                Id = x.Id,
                BrewStepId = query.BrewStepId,
                Label = x.Label,
                ValueAsString = x.Value,
                ValueType = "string",
                Units = x.Units,
                Optional = x.Optional
            });
            return floatValues.Union(intValues).Union(stringValues).OrderBy(x => x.Label).ToList();
        }
    }
}
