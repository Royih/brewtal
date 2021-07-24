namespace Brewtal2.Storage
{
    public interface IStorageRepository
    {
        void InitializeDb();
        void RegisterStartup();
        Session GetCurrentSession();
        Session SetNewTargetTemp(double newTargetTemp, double actualTemp);
        void LogTemp(double newTemp);
    }
}