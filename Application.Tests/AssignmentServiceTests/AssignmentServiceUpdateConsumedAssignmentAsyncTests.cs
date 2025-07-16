using Application.IPublishers;
using Application.Services;
using Domain.Factory.AssignmentFactory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Moq;

namespace Application.Tests.AssignmentServiceTests;

public class UpdateConsumedAssignmentAsyncTests
{
    [Fact]
    public async Task UpdateConsumedAssignmentAsync_ShouldUpdateAndReturnAssignment_IfItExists()
    {
        // Arrange
        var assignmentId = Guid.NewGuid();
        var oldCollaboratorId = Guid.NewGuid();
        var newCollaboratorId = Guid.NewGuid();
        var oldDeviceId = Guid.NewGuid();
        var newDeviceId = Guid.NewGuid();
        var oldPeriod = new PeriodDate(new DateOnly(2025, 7, 1), new DateOnly(2025, 7, 10));
        var newPeriod = new PeriodDate(new DateOnly(2025, 8, 1), new DateOnly(2025, 8, 10));

        var assignmentMock = new Mock<IAssignment>();
        assignmentMock.Setup(a => a.Id).Returns(assignmentId);

        assignmentMock.Setup(a => a.UpdateCollaborator(It.IsAny<Guid>()))
                      .Verifiable();
        assignmentMock.Setup(a => a.UpdateDevice(It.IsAny<Guid>()))
                      .Verifiable();
        assignmentMock.Setup(a => a.UpdatePeriodDate(It.IsAny<PeriodDate>()))
                      .Verifiable();

        var assignmentRepo = new Mock<IAssignmentRepository>();
        assignmentRepo.Setup(r => r.GetAssignmentByIdAsync(assignmentId)).ReturnsAsync(assignmentMock.Object);
        assignmentRepo.Setup(r => r.UpdateAssignmentAsync(It.IsAny<IAssignment>()))
                      .ReturnsAsync(assignmentMock.Object);

        var assignmentFactory = new Mock<IAssignmentFactory>();
        var deviceRepository = new Mock<IDeviceRepository>();
        var collaboratorRepository = new Mock<ICollaboratorRepository>();
        var publisher = new Mock<IMessagePublisher>();

        var service = new AssignmentService(assignmentRepo.Object, assignmentFactory.Object, publisher.Object, deviceRepository.Object, collaboratorRepository.Object);

        // Act
        var result = await service.UpdateConsumedAssignmentAsync(assignmentId, newCollaboratorId, newDeviceId, newPeriod);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(assignmentId, result!.Id);
        assignmentMock.Verify(a => a.UpdateCollaborator(newCollaboratorId), Times.Once);
        assignmentMock.Verify(a => a.UpdateDevice(newDeviceId), Times.Once);
        assignmentMock.Verify(a => a.UpdatePeriodDate(newPeriod), Times.Once);
        assignmentRepo.Verify(r => r.UpdateAssignmentAsync(assignmentMock.Object), Times.Once);
    }

    [Fact]
    public async Task UpdateConsumedAssignmentAsync_ShouldReturnNull_IfAssignmentDoesNotExist()
    {
        // Arrange
        var assignmentId = Guid.NewGuid();
        var collaboratorId = Guid.NewGuid();
        var deviceId = Guid.NewGuid();
        var period = new PeriodDate(new DateOnly(2025, 8, 1), new DateOnly(2025, 8, 10));

        var assignmentRepo = new Mock<IAssignmentRepository>();
        assignmentRepo.Setup(r => r.GetAssignmentByIdAsync(assignmentId)).ReturnsAsync((IAssignment?)null);

        var assignmentFactory = new Mock<IAssignmentFactory>();
        var deviceRepository = new Mock<IDeviceRepository>();
        var collaboratorRepository = new Mock<ICollaboratorRepository>();
        var publisher = new Mock<IMessagePublisher>();

        var service = new AssignmentService(assignmentRepo.Object, assignmentFactory.Object, publisher.Object, deviceRepository.Object, collaboratorRepository.Object);
        // Act
        var result = await service.UpdateConsumedAssignmentAsync(assignmentId, collaboratorId, deviceId, period);

        // Assert
        Assert.Null(result);
        assignmentRepo.Verify(r => r.UpdateAssignmentAsync(It.IsAny<IAssignment>()), Times.Never);
    }
}
