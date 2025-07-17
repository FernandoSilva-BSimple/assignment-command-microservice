using Application.Interfaces;
using Domain.Messages;
using Domain.Models;
using InterfaceAdapters.Consumers;
using MassTransit;
using Moq;
using Xunit;

namespace InterfaceAdapters.Tests.ConsumerTests;

public class AssignmentUpdatedConsumerTests
{
    [Fact]
    public async Task Consume_ShouldCallUpdateConsumedAssignmentAsync_WithCorrectData()
    {
        // Arrange
        var serviceDouble = new Mock<IAssignmentService>();
        var consumer = new AssignmentUpdatedConsumer(serviceDouble.Object);

        var assignmentId = Guid.NewGuid();
        var collaboratorId = Guid.NewGuid();
        var deviceId = Guid.NewGuid();
        var start = new DateOnly(2025, 7, 1);
        var end = new DateOnly(2025, 7, 10);

        var message = new AssignmentUpdatedMessage(assignmentId, deviceId, collaboratorId, start, end);

        var context = Mock.Of<ConsumeContext<AssignmentUpdatedMessage>>(c => c.Message == message);

        // Act
        await consumer.Consume(context);

        // Assert
        serviceDouble.Verify(s =>
            s.UpdateConsumedAssignmentAsync(
                assignmentId,
                collaboratorId,
                deviceId,
                It.Is<PeriodDate>(p => p.InitDate == start && p.FinalDate == end)
            ),
            Times.Once
        );
    }

}