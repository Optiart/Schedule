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

        public Tab(Tabs dto)
        {
            Id = dto.id;
            NumberOfDevices = dto.number_of_devices;
            DeviceType = (DeviceType)dto.device_type;
            DeviceProductivities = JsonConvert.DeserializeObject<decimal[]>(dto.productivity);
            NumberOfPalleteRows = dto.number_of_palletes;
            NumberOfWorkPerRow = dto.number_of_work;
            DurationByWork = JsonConvert.DeserializeObject<decimal[,]>(dto.work_per_pallete);
        }
    }
}