using Application.Interfaces;
using Domain.Messages;
using MassTransit;

namespace InterfaceAdapters.Consumers;

public class DeviceCreatedConsumer : IConsumer<DeviceCreatedMessage>
{
    private readonly IDeviceService _deviceService;

    public DeviceCreatedConsumer(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }

    public Task Consume(ConsumeContext<DeviceCreatedMessage> context)
    {
        var deviceId = context.Message.Id;
        return _deviceService.AddConsumedDeviceAsync(deviceId);
    }
}