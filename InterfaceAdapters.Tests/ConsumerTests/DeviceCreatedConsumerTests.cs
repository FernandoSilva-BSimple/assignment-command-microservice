using Application.Interfaces;
using Domain.Messages;
using InterfaceAdapters.Consumers;
using MassTransit;
using Moq;
using Xunit;

namespace InterfaceAdapters.Tests.ConsumerTests;

public class DeviceCreatedConsumerTests
{
    [Fact]
    public async Task Consume_ShouldCallAddConsumedDeviceAsync_WithCorrectData()
    {
        // Arrange
        var serviceDouble = new Mock<IDeviceService>();
        var consumer = new DeviceCreatedConsumer(serviceDouble.Object);

        var deviceDescription = "Description";
        var deviceBrand = "Brand";
        var deviceModel = "Model";
        var deviceSerialNumber = "SerialNumber";
        var message = new DeviceCreatedMessage(Guid.NewGuid(), deviceDescription, deviceBrand, deviceModel, deviceSerialNumber, null);
        var context = Mock.Of<ConsumeContext<DeviceCreatedMessage>>(c => c.Message == message);

        // Act
        await consumer.Consume(context);

        // Assert
        serviceDouble.Verify(s => s.AddConsumedDeviceAsync(message.Id), Times.Once);
    }
}
