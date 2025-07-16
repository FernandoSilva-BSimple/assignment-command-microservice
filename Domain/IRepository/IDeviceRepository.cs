using Domain.Interfaces;

namespace Domain.IRepository;

public interface IDeviceRepository
{
    Task<bool> Exists(Guid id);
    Task<IDevice> AddDeviceAsync(IDevice device);
}