namespace Schedule.Domain.Models
{
    public class DeviceGraphRow
    {
        public decimal PalleteWork { get; set; }

        public decimal End { get; set; }

        public decimal Duration { get; set; }

        public decimal Start => End - Duration;
    }
}