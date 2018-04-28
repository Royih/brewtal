namespace Brewtal.CQRS
{
    public interface IAggregateRootFactory
    {
        BrewAR GetBrewById(int brewId);
    }
}