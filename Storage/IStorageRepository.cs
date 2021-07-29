using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brewtal2.Storage
{
    public interface IStorageRepository
    {
        void InitializeDb();
        void RegisterStartup();
        Session GetCurrentSession();
        Templog GetLatestTempLog(int sessionId);
        Session SetNewTargetTemp(double newTargetTemp, double actualTemp);
        Templog LogTemp(double newTemp);
        Task<Session> GetSessionAsync(int sessionId);
        Task<IEnumerable<Session>> ListSessions();
    }
}