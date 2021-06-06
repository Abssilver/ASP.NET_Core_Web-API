using System;

namespace MetricsManagerClient.DataLayer.Interfaces
{
    public interface IAppModel
    {
        public bool IsFollowAgent { get; set; }
        public string ManagerBaseAddress { get; set; }
        public DateTimeOffset From { get; set; }
        public DateTimeOffset To { get; set; }
        public int AgentId { get; set; }
    }
}