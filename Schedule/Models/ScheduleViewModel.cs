using Schedule.Models.Enums;

namespace Schedule.Models
{
    public class ScheduleViewModel
    {
        public int Id { get; set; }

        [Positive]
        public int NumberOfDevices { get; set; }

        public DeviceType DeviceType { get; set; }

        public decimal[] DeviceProductivities { get; set; }

        [Positive]
        public int NumberOfPalleteRows { get; set; }

        [Positive]
        public int NumberOfWorkPerRow { get; set; }

        public decimal[,] DurationByWork { get; set; }
    }
}