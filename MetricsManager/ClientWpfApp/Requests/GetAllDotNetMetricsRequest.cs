﻿using System;

namespace MetricsManagerClient.Requests
{
    public class GetAllDotNetMetricsRequest
    {
        public DateTimeOffset FromTime { get; set; }
        
        public DateTimeOffset ToTime { get; set; }
    }
}