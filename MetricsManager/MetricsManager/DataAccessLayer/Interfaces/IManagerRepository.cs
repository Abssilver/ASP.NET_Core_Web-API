using System;
using System.Collections.Generic;
using Core.Interfaces;

namespace MetricsManager.DataAccessLayer.Interfaces
{
    public interface IManagerRepository<T>:IRepository<T> where T: class
    {
        DateTimeOffset GetLastRecordDate(int agentId);
        IList<T> GetByTimePeriodFromAgent(DateTimeOffset from, DateTimeOffset to, int agentId);
    }
}