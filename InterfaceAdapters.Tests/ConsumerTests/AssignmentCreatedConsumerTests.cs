using Application.Interfaces;
using Domain.Messages;
using Domain.Models;
using InterfaceAdapters.Consumers;
using MassTransit;
using Moq;
using Xunit;

namespace InterfaceAdapters.Tests.ConsumerTests;

public class AssignmentCreatedTests
{
    [Fact]
    public async Task Consume_ShouldCallAddConsumedAssignmentAsync_WithCorrectData()
    {
        // Arrange
        var serviceDouble = new Mock<IAssignmentService>();
        var consumer = new AssignmentCreatedConsumer(serviceDouble.Object);

        var message = new AssignmentCreatedMessage(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>());

        var context = Mock.Of<ConsumeContext<AssignmentCreatedMessage>>(c => c.Message == message);

        //act
        await consumer.Consume(context);

        // Assert
        serviceDouble.Verify(s => s.AddConsumedAssignmentAsync(message.AssignmentId, message.DeviceId, message.CollaboratorId, It.IsAny<PeriodDate>()), Times.Once);
    }
}