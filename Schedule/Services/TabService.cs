using System.Linq;
using Newtonsoft.Json;
using Schedule.DataAccess;
using Schedule.Models;

namespace Schedule.Services
{
    public class TabService
    {
        private IRepository _repository;

        public TabService()
        {
            _repository = new Repository();
        }

        public Tab[] GetAll()
        {
            Tabs[] tabsDto = _repository.GetAll();
            return tabsDto.Select(t => new Tab(t)).ToArray();
        }

        public void Save(TabViewModel tabModel)
        {
            var dto = new Tabs
            {
                device_type = (byte)tabModel.DeviceType,
                number_of_devices = tabModel.NumberOfDevices,
                number_of_palletes = tabModel.NumberOfPalleteRows,
                number_of_work = tabModel.NumberOfWorkPerRow,
                productivity = JsonConvert.SerializeObject(tabModel.DeviceProductivities),
                work_per_pallete = JsonConvert.SerializeObject(tabModel.DurationByWork)
            };

            _repository.Save(dto);
        }
    }
}