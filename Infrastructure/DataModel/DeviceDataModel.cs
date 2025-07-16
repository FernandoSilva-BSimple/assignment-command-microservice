using Domain.Interfaces;
using Domain.Visitors;

namespace Infrastructure.DataModel;

public class DeviceDataModel : IDeviceVisitor
{
    public Guid Id { get; set; }

    public DeviceDataModel(IDevice device)
    {
        Id = device.Id;
    }

    public DeviceDataModel() { }
}