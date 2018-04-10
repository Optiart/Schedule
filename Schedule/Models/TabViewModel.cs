using Schedule.Models.Enums;

namespace Schedule.Models
{
    public class TabViewModel
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

        public TabViewModel()
        {
        }

        public TabViewModel(Tab tab)
        {
            Id = tab.Id;
            NumberOfDevices = tab.NumberOfDevices;
            DeviceType = tab.DeviceType;
            DeviceProductivities = tab.DeviceProductivities;
            NumberOfPalleteRows = tab.NumberOfPalleteRows;
            NumberOfWorkPerRow = tab.NumberOfWorkPerRow;
            DurationByWork = tab.DurationByWork;
        }
    }
}