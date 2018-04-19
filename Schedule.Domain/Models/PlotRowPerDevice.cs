namespace Schedule.Domain.Models
{
    public class PlotRowPerDevice
    {
        public decimal PalletWork { get; set; }

        public decimal End { get; set; }

        public decimal Duration { get; set; }

        public decimal Start => End - Duration;
    }
}