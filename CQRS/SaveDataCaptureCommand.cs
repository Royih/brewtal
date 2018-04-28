using System;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using Brewtal.Database;
using Brewtal.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Brewtal.CQRS
{

    public class SaveDataCaptureCommand : IRequest
    {
        public DataCaptureValueDto[] Values { get; set; }
    }

    public class SaveDataCaptureCommandHandler : RequestHandler<SaveDataCaptureCommand>
    {
        private readonly BrewtalContext _db;

        public SaveDataCaptureCommandHandler(BrewtalContext db)
        {
            _db = db;
        }

        protected override void HandleCore(SaveDataCaptureCommand command)
        {
            foreach (var value in command.Values)
            {
                if (value.ValueType == "float")
                {
                    var thisValue = _db.DataCaptureFloatValues.Single(x => x.Id == value.Id);
                    if (!string.IsNullOrEmpty(value.ValueAsString))
                    {
                        float floatValue;
                        if (float.TryParse(value.ValueAsString, out floatValue))
                        {
                            thisValue.Value = floatValue;
                        }
                    }
                    else
                    {
                        thisValue.Value = null;
                    }
                }
                if (value.ValueType == "int")
                {
                    var thisValue = _db.DataCaptureIntValues.Single(x => x.Id == value.Id);
                    if (!string.IsNullOrEmpty(value.ValueAsString))
                    {
                        int intValue;
                        if (int.TryParse(value.ValueAsString, out intValue))
                        {
                            thisValue.Value = intValue;
                        }
                    }
                    else
                    {
                        thisValue.Value = null;
                    }
                }
                if (value.ValueType == "string")
                {
                    var thisValue = _db.DataCaptureStringValues.Single(x => x.Id == value.Id);
                    if (!string.IsNullOrEmpty(value.ValueAsString))
                    {
                        thisValue.Value = value.ValueAsString;
                    }
                    else
                    {
                        thisValue.Value = null;
                    }
                }
            }
            _db.SaveChanges();
        }
    }
}
