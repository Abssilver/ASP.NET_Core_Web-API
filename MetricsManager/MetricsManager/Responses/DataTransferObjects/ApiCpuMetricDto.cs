﻿
using System;

namespace MetricsManager.Responses.DataTransferObjects
{
    public class ApiCpuMetricDto
    {
        public DateTimeOffset Time { get; set; }
        public int Value { get; set; }
        public int Id { get; set; }
        public int AgentId { get; set; }
    }

}