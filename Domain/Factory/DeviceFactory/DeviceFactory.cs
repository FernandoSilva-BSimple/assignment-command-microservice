using Domain.Models;
using Domain.Visitors;

namespace Domain.Factory.DeviceFactory;

public class DeviceFactory : IDeviceFactory
{
    public Device Create(Guid id)
    {
        return new Device(id);
    }

    public Device Create(IDeviceVisitor deviceVisitor)
    {
        return new Device(deviceVisitor.Id);
    }
}