using System.Linq;
using Newtonsoft.Json;
using Schedule.DataAccess;
using Schedule.Models;

namespace Schedule.Domain
{
    internal class TabService : ITabService
    {
        private ITabRepository _repository;

        public TabService(ITabRepository repository)
        {
            _repository = repository;
        }

        public int[] GetAllTabIds()
        {
            return _repository.GetAllTabIds();
        }

        public Tab[] GetAll()
        {
            TabDto[] tabsDto = _repository.GetAll();
            return tabsDto.Select(t => new Tab(t)).ToArray();
        }

        public int Save(Tab tabModel)
        {
            var dto = new TabDto
            {
                DeviceType = (byte)tabModel.DeviceType,
                NumberOfDevices = tabModel.NumberOfDevices,
                NumberOfPalletes = tabModel.NumberOfPalleteRows,
                NumberOfWork = tabModel.NumberOfWorkPerRow,
                Productivity = JsonConvert.SerializeObject(tabModel.DeviceProductivities),
                WorkPerPallete = JsonConvert.SerializeObject(tabModel.DurationByWork)
            };

            _repository.Save(dto);
            return dto.Id;
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }
    }
}