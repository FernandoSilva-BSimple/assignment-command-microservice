using Application.Interfaces;
using Domain.Factory.DeviceFactory;
using Domain.Interfaces;
using Domain.IRepository;

namespace Application.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly IDeviceFactory _deviceFactory;

        public DeviceService(IDeviceRepository deviceRepository, IDeviceFactory deviceFactory)
        {
            _deviceRepository = deviceRepository;
            _deviceFactory = deviceFactory;
        }

        public async Task<IDevice> AddConsumedDeviceAsync(Guid deviceId)
        {
            var newDevice = _deviceFactory.Create(deviceId);

            return await _deviceRepository.AddDeviceAsync(newDevice);
        }
    }
}