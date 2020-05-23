using System.Collections.Generic;
using Brewtal2.Brews.Models;

namespace Brewtal2.Brews
{
    public interface IBrewRepository
    {
        void SeedBrewstepTemplates();
        IEnumerable<Brew> ListBrews();
        Brew GetBrew(string id = null);
        Brew SaveBrew(Brew brewToSave);
        void DeleteBrew(Brew brewToDelete);
    }
}