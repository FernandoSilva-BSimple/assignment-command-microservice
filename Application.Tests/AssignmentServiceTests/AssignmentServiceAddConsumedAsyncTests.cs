using Application.IPublishers;
using Application.Services;
using Domain.Factory.AssignmentFactory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Moq;

namespace Application.Tests.AssignmentServiceTests;

public class AddConsumedAssignmentAsyncTests
{
    [Fact]
    public async Task AddConsumedAssignmentAsync_ShouldReturnExistingAssignment_IfItExists()
    {
        // Arrange
        var assignmentId = Guid.NewGuid();
        var deviceId = Guid.NewGuid();
        var collaboratorId = Guid.NewGuid();
        var period = new PeriodDate(new DateOnly(2025, 7, 1), new DateOnly(2025, 7, 10));

        var existingAssignment = Mock.Of<IAssignment>(a =>
            a.Id == assignmentId &&
            a.DeviceId == deviceId &&
            a.CollaboratorId == collaboratorId &&
            a.PeriodDate == period
        );

        var assignmentRepo = new Mock<IAssignmentRepository>();
        assignmentRepo.Setup(r => r.GetAssignmentByIdAsync(assignmentId)).ReturnsAsync(existingAssignment);

        var assignmentFactory = new Mock<IAssignmentFactory>();
        var deviceRepositoryMock = new Mock<IDeviceRepository>();
        var collaboratorRepositoryMock = new Mock<ICollaboratorRepository>();
        var publisherMock = new Mock<IMessagePublisher>();

        var service = new AssignmentService(assignmentRepo.Object, assignmentFactory.Object, publisherMock.Object, deviceRepositoryMock.Object, collaboratorRepositoryMock.Object);

        // Act
        var result = await service.AddConsumedAssignmentAsync(assignmentId, collaboratorId, deviceId, period);

        // Assert
        Assert.Equal(existingAssignment, result);
        assignmentFactory.Verify(f => f.Create(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<PeriodDate>()), Times.Never);
        assignmentRepo.Verify(r => r.CreateAssignmentAsync(It.IsAny<IAssignment>()), Times.Never);
    }

    [Fact]
    public async Task AddConsumedAssignmentAsync_ShouldCreateAndPersistAssignment_IfItDoesNotExist()
    {
        // Arrange
        var assignmentId = Guid.NewGuid();
        var deviceId = Guid.NewGuid();
        var collaboratorId = Guid.NewGuid();
        var period = new PeriodDate(new DateOnly(2025, 7, 1), new DateOnly(2025, 7, 10));

        var assignmentRepo = new Mock<IAssignmentRepository>();
        assignmentRepo.Setup(r => r.GetAssignmentByIdAsync(assignmentId)).ReturnsAsync((IAssignment?)null);

        var newAssignment = Mock.Of<IAssignment>(a =>
            a.Id == assignmentId &&
            a.DeviceId == deviceId &&
            a.CollaboratorId == collaboratorId &&
            a.PeriodDate == period
        );

        var assignmentFactory = new Mock<IAssignmentFactory>();
        assignmentFactory.Setup(f => f.Create(assignmentId, collaboratorId, deviceId, period)).Returns(newAssignment);

        assignmentRepo.Setup(r => r.CreateAssignmentAsync(newAssignment)).ReturnsAsync(newAssignment);

        var deviceRepositoryMock = new Mock<IDeviceRepository>();
        var collaboratorRepositoryMock = new Mock<ICollaboratorRepository>();
        var publisherMock = new Mock<IMessagePublisher>();

        var service = new AssignmentService(assignmentRepo.Object, assignmentFactory.Object, publisherMock.Object, deviceRepositoryMock.Object, collaboratorRepositoryMock.Object);

        // Act
        var result = await service.AddConsumedAssignmentAsync(assignmentId, collaboratorId, deviceId, period);

        // Assert
        Assert.Equal(newAssignment, result);
        assignmentFactory.Verify(f => f.Create(assignmentId, collaboratorId, deviceId, period), Times.Once);
        assignmentRepo.Verify(r => r.CreateAssignmentAsync(newAssignment), Times.Once);
    }
}
