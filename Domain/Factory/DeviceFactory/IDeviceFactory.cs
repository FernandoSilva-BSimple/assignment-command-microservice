using Domain.Models;
using Domain.Visitors;

namespace Domain.Factory.DeviceFactory;

public interface IDeviceFactory
{
    Device Create(Guid id);
    Device Create(IDeviceVisitor visitor);
}