using Domain.Interfaces;
using Domain.Models;

namespace Application.Interfaces;

public interface IDeviceService
{
    Task<IDevice> AddConsumedDeviceAsync(Guid deviceId);
}