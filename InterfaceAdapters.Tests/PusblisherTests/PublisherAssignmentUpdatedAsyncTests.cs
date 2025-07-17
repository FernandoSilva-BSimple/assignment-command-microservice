using Domain.Interfaces;
using Domain.Messages;
using Domain.Models;
using InterfaceAdapters.Publisher;
using MassTransit;
using Moq;
using Xunit;

namespace InterfaceAdapters.Tests.Publishertests;

public class PublishAssignmentUpdatedAsyncTests
{
    [Fact]
    public async Task PublishAssignmentUpdatedAsync_ShouldCallPublish_WithCorrectData()
    {
        // Arrange
        var publishEndpointDouble = new Mock<IPublishEndpoint>();
        var sendEndpointDouble = new Mock<ISendEndpointProvider>();
        var publisher = new MassTransitPublisher(publishEndpointDouble.Object, sendEndpointDouble.Object);

        var assignmentId = Guid.NewGuid();
        var deviceId = Guid.NewGuid();
        var collaboratorId = Guid.NewGuid();
        var initDate = new DateOnly(2025, 7, 1);
        var finalDate = new DateOnly(2025, 7, 10);
        var period = new PeriodDate(initDate, finalDate);

        // Act
        await publisher.PublishAssignmentUpdatedAsync(assignmentId, deviceId, collaboratorId, period);

        // Assert
        publishEndpointDouble.Verify(p => p.Publish(
     It.Is<AssignmentUpdatedMessage>(m =>
         m.AssignmentId == assignmentId &&
         m.DeviceId == deviceId &&
         m.CollaboratorId == collaboratorId &&
         m.StartDate == initDate &&
         m.EndDate == finalDate
     ),
     CancellationToken.None
 ), Times.Once);
    }
}