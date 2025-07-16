using AutoMapper;
using Domain.Factory.DeviceFactory;
using Domain.Interfaces;
using Infrastructure.DataModel;

namespace Infrastructure.Resolvers;

public class DeviceDataModelConverter : ITypeConverter<DeviceDataModel, IDevice>
{
    private readonly IDeviceFactory _factory;

    public DeviceDataModelConverter(IDeviceFactory factory)
    {
        _factory = factory;
    }

    public IDevice Convert(DeviceDataModel source, IDevice destination, ResolutionContext context)
    {
        return _factory.Create(source);
    }
}