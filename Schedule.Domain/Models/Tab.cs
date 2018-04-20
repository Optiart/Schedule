using Schedule.DataAccess;
using Schedule.Domain.Models;
using Newtonsoft.Json;

namespace Schedule.Models
{
    public class Tab
    {
        public int Id { get; set; }

        public int NumberOfDevices { get; set; }

        public DeviceType DeviceType { get; set; }

        public decimal[] DeviceProductivities { get; set; }

        public int NumberOfPalleteRows { get; set; }

        public int NumberOfWorkPerRow { get; set; }

        public decimal[,] DurationByWork { get; set; }

        public Tab()
        {
        }

        public Tab(TabDto dto)
        {
            Id = dto.Id;
            NumberOfDevices = dto.NumberOfDevices;
            DeviceType = (DeviceType)dto.DeviceType;
            DeviceProductivities = JsonConvert.DeserializeObject<decimal[]>(dto.Productivity);
            NumberOfPalleteRows = dto.NumberOfPalletes;
            NumberOfWorkPerRow = dto.NumberOfWork;
            DurationByWork = JsonConvert.DeserializeObject<decimal[,]>(dto.WorkPerPallete);
        }
    }
}