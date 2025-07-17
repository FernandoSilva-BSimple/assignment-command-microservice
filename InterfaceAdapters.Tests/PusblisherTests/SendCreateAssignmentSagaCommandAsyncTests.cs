using Contracts.Commands;
using InterfaceAdapters.Publisher;
using MassTransit;
using Moq;
using Xunit;

namespace InterfaceAdapters.Tests.PublisherTests;

public class MassTransitPublisherTests
{
    [Fact]
    public async Task SendCreateAssignmentSagaCommandAsync_ShouldSendCommandToCorrectQueue()
    {
        // Arrange
        var instanceId = InstanceInfo.InstanceId;

        var command = new CreateRequestedAssignmentCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            new DateOnly(2025, 7, 20),
            new DateOnly(2025, 7, 30),
            "Laptop",
            "Dell",
            "XPS 15",
            "123ABC456"
        );

        var sendEndpointMock = new Mock<ISendEndpoint>();
        var sendEndpointProviderMock = new Mock<ISendEndpointProvider>();

        sendEndpointProviderMock
            .Setup(p => p.GetSendEndpoint(new Uri($"queue:assignments-cmd-saga-{instanceId}")))
            .ReturnsAsync(sendEndpointMock.Object);

        var publishEndpointMock = new Mock<IPublishEndpoint>();

        var publisher = new MassTransitPublisher(publishEndpointMock.Object, sendEndpointProviderMock.Object);

        // Act
        await publisher.SendCreateAssignmentSagaCommandAsync(command);

        // Assert
        sendEndpointProviderMock.Verify(p => p.GetSendEndpoint(new Uri($"queue:assignments-cmd-saga-{instanceId}")), Times.Once);
        sendEndpointMock.Verify(e => e.Send(command, It.IsAny<CancellationToken>()), Times.Once);
    }
}
