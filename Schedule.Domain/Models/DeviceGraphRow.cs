namespace Schedule.Domain.Models
{
    public class DeviceGraphRow
    {
        public decimal PalleteWork { get; set; }

        public int Start { get; set; }

        public int End { get; set; }

        public int Duration => End - Start;
    }
}