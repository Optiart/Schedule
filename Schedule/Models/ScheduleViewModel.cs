﻿using Schedule.Models.Enums;
using System.Collections.Generic;

namespace Schedule.Models
{
    public class ScheduleViewModel
    {
        [Positive]
        public int NumberOfDevices { get; set; }

        public DeviceType DeviceType { get; set; }

        public Dictionary<decimal, decimal> ProductivityCoefByDevice { get; set; }

        [Positive]
        public int NumberOfPalleteRows { get; set; }

        [Positive]
        public int NumberOfWorkPerRow { get; set; }

        public Dictionary<decimal, decimal> DurationByWork { get; set; }
    }
}