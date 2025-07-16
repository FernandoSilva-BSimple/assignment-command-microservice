using Domain.Factory.AssignmentFactory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Domain.Visitors;
using Moq;

namespace Domain.Tests.AssignmentDomainTests;

public class AssignmentFactoryTests
{
    [Fact]
    public void WhenCreatingAssignmentWithValidFieldsAndId_ThenAssignmentIsCreated()
    {
        // Arrange
        var assignmentRepositoryMock = new Mock<IAssignmentRepository>();
        var collabRepositoryMock = new Mock<ICollaboratorRepository>();
        var deviceRepositoryMock = new Mock<IDeviceRepository>();

        var assignmentFactory = new AssignmentFactory(assignmentRepositoryMock.Object, collabRepositoryMock.Object, deviceRepositoryMock.Object);

        // Act
        var assignment = assignmentFactory.Create(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<PeriodDate>());

        // Assert
        Assert.NotNull(assignment);
    }

    [Fact]
    public async Task WhenCreatingAssignmentWithValidFields_ThenAssignmentIsCreated()
    {
        // Arrange
        var collaboratorId = Guid.NewGuid();
        var deviceId = Guid.NewGuid();

        var collabPeriodStart = new DateTime(2025, 7, 1);
        var collabPeriodEnd = new DateTime(2025, 7, 31);
        var collabPeriodDateTime = new PeriodDateTime(collabPeriodStart, collabPeriodEnd);

        var assignmentPeriod = new PeriodDate(new DateOnly(2025, 7, 10), new DateOnly(2025, 7, 20));

        var collaborator = new Mock<ICollaborator>();
        collaborator.Setup(c => c.Id).Returns(collaboratorId);
        collaborator.Setup(c => c.PeriodDateTime).Returns(collabPeriodDateTime);

        var assignmentRepositoryMock = new Mock<IAssignmentRepository>();
        var collabRepositoryMock = new Mock<ICollaboratorRepository>();
        var deviceRepositoryMock = new Mock<IDeviceRepository>();

        assignmentRepositoryMock
            .Setup(r => r.ExistsWithDeviceAndOverlappingPeriod(deviceId, assignmentPeriod))
            .ReturnsAsync(false);

        collabRepositoryMock.Setup(cr => cr.GetByIdAsync(collaboratorId)).ReturnsAsync(collaborator.Object);

        deviceRepositoryMock.Setup(r => r.Exists(deviceId)).ReturnsAsync(true);

        var assignmentFactory = new AssignmentFactory(assignmentRepositoryMock.Object, collabRepositoryMock.Object, deviceRepositoryMock.Object);

        // Act
        var result = await assignmentFactory.Create(deviceId, collaboratorId, assignmentPeriod);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(deviceId, result.DeviceId);
        Assert.Equal(collaboratorId, result.CollaboratorId);
        Assert.Equal(assignmentPeriod, result.PeriodDate);
    }

    [Fact]
    public async Task WhenCreatingAssignmentWithNonExistingCollaborator_ThenShouldThrowException()
    {
        // Arrange
        var collaboratorId = Guid.NewGuid();
        var deviceId = Guid.NewGuid();
        var assignmentPeriod = new PeriodDate(new DateOnly(2025, 7, 10), new DateOnly(2025, 7, 20));

        var assignmentRepositoryMock = new Mock<IAssignmentRepository>();
        var collabRepositoryMock = new Mock<ICollaboratorRepository>();
        var deviceRepositoryMock = new Mock<IDeviceRepository>();

        collabRepositoryMock
            .Setup(r => r.GetByIdAsync(collaboratorId))
            .ReturnsAsync((Collaborator)null!);

        var assignmentFactory = new AssignmentFactory(
            assignmentRepositoryMock.Object,
            collabRepositoryMock.Object,
            deviceRepositoryMock.Object
        );

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            assignmentFactory.Create(deviceId, collaboratorId, assignmentPeriod));

        Assert.Equal("Collaborator not found", exception.Message);
    }

    [Fact]
    public async Task WhenAssignmentPeriodIsOutsideCollaboratorPeriod_ThenShouldThrowException()
    {
        // Arrange
        var collaboratorId = Guid.NewGuid();
        var deviceId = Guid.NewGuid();

        var collabPeriodStart = new DateTime(2025, 7, 1);
        var collabPeriodEnd = new DateTime(2025, 7, 5); // colaborador disponível só até dia 5
        var collabPeriodDateTime = new PeriodDateTime(collabPeriodStart, collabPeriodEnd);

        var assignmentPeriod = new PeriodDate(new DateOnly(2025, 7, 10), new DateOnly(2025, 7, 20)); // fora do período

        var collaboratorMock = new Mock<ICollaborator>();
        collaboratorMock.Setup(c => c.Id).Returns(collaboratorId);
        collaboratorMock.Setup(c => c.PeriodDateTime).Returns(collabPeriodDateTime);

        var assignmentRepositoryMock = new Mock<IAssignmentRepository>();
        assignmentRepositoryMock
            .Setup(r => r.ExistsWithDeviceAndOverlappingPeriod(deviceId, assignmentPeriod))
            .ReturnsAsync(false);

        var collabRepositoryMock = new Mock<ICollaboratorRepository>();
        collabRepositoryMock
            .Setup(r => r.GetByIdAsync(collaboratorId))
            .ReturnsAsync(collaboratorMock.Object);

        var deviceRepositoryMock = new Mock<IDeviceRepository>();
        deviceRepositoryMock
            .Setup(r => r.Exists(deviceId))
            .ReturnsAsync(true);

        var assignmentFactory = new AssignmentFactory(
            assignmentRepositoryMock.Object,
            collabRepositoryMock.Object,
            deviceRepositoryMock.Object
        );

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            assignmentFactory.Create(deviceId, collaboratorId, assignmentPeriod));

        Assert.Equal("Assignment period must be within the collaborator's active period", exception.Message);
    }

    [Fact]
    public async Task WhenCreatingAssignmentWithNonExistingDevice_ThenShouldThrowException()
    {
        // Arrange
        var collaboratorId = Guid.NewGuid();
        var deviceId = Guid.NewGuid();

        var collabPeriodStart = new DateTime(2025, 7, 1);
        var collabPeriodEnd = new DateTime(2025, 7, 31);
        var collabPeriodDateTime = new PeriodDateTime(collabPeriodStart, collabPeriodEnd);

        var assignmentPeriod = new PeriodDate(new DateOnly(2025, 7, 10), new DateOnly(2025, 7, 20));

        var collaboratorMock = new Mock<ICollaborator>();
        collaboratorMock.Setup(c => c.Id).Returns(collaboratorId);
        collaboratorMock.Setup(c => c.PeriodDateTime).Returns(collabPeriodDateTime);

        var assignmentRepositoryMock = new Mock<IAssignmentRepository>();
        assignmentRepositoryMock
            .Setup(r => r.ExistsWithDeviceAndOverlappingPeriod(deviceId, assignmentPeriod))
            .ReturnsAsync(false);

        var collabRepositoryMock = new Mock<ICollaboratorRepository>();
        collabRepositoryMock
            .Setup(r => r.GetByIdAsync(collaboratorId))
            .ReturnsAsync(collaboratorMock.Object);

        var deviceRepositoryMock = new Mock<IDeviceRepository>();
        deviceRepositoryMock
            .Setup(r => r.Exists(deviceId))
            .ReturnsAsync(false);

        var assignmentFactory = new AssignmentFactory(
            assignmentRepositoryMock.Object,
            collabRepositoryMock.Object,
            deviceRepositoryMock.Object
        );

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            assignmentFactory.Create(deviceId, collaboratorId, assignmentPeriod));

        Assert.Equal("Device not found", exception.Message);
    }


    [Fact]
    public async Task WhenCreatingAssignmentWithOverlappingDevicePeriod_ThenShouldThrowException()
    {
        // Arrange
        var collaboratorId = Guid.NewGuid();
        var deviceId = Guid.NewGuid();

        var collabPeriodStart = new DateTime(2025, 7, 1);
        var collabPeriodEnd = new DateTime(2025, 7, 31);
        var collabPeriodDateTime = new PeriodDateTime(collabPeriodStart, collabPeriodEnd);

        var assignmentPeriod = new PeriodDate(new DateOnly(2025, 7, 10), new DateOnly(2025, 7, 20));

        var collaboratorMock = new Mock<ICollaborator>();
        collaboratorMock.Setup(c => c.Id).Returns(collaboratorId);
        collaboratorMock.Setup(c => c.PeriodDateTime).Returns(collabPeriodDateTime);

        var assignmentRepositoryMock = new Mock<IAssignmentRepository>();
        assignmentRepositoryMock
            .Setup(r => r.ExistsWithDeviceAndOverlappingPeriod(deviceId, assignmentPeriod))
            .ReturnsAsync(true);

        var collabRepositoryMock = new Mock<ICollaboratorRepository>();
        collabRepositoryMock
            .Setup(r => r.GetByIdAsync(collaboratorId))
            .ReturnsAsync(collaboratorMock.Object);

        var deviceRepositoryMock = new Mock<IDeviceRepository>();
        deviceRepositoryMock
            .Setup(r => r.Exists(deviceId))
            .ReturnsAsync(true);

        var assignmentFactory = new AssignmentFactory(
            assignmentRepositoryMock.Object,
            collabRepositoryMock.Object,
            deviceRepositoryMock.Object
        );

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            assignmentFactory.Create(deviceId, collaboratorId, assignmentPeriod));

        Assert.Equal("Device already has an assigment overlapping with this period", exception.Message);
    }

    [Fact]
    public void WhenCreatingAssignmentFromVisitor_ThenAssignmentIsCreatedCorrectly()
    {
        // Arrange
        var assignmentRepositoryMock = new Mock<IAssignmentRepository>();
        var collabRepositoryMock = new Mock<ICollaboratorRepository>();
        var deviceRepositoryMock = new Mock<IDeviceRepository>();

        var visitorMock = new Mock<IAssignmentVisitor>();
        visitorMock.Setup(v => v.Id).Returns(It.IsAny<Guid>());
        visitorMock.Setup(v => v.DeviceId).Returns(It.IsAny<Guid>());
        visitorMock.Setup(v => v.CollaboratorId).Returns(It.IsAny<Guid>());
        visitorMock.Setup(v => v.PeriodDate).Returns(new PeriodDate(new DateOnly(2025, 7, 10), new DateOnly(2025, 7, 20)));

        var assignmentFactory = new AssignmentFactory(assignmentRepositoryMock.Object, collabRepositoryMock.Object, deviceRepositoryMock.Object);

        // Act
        var result = assignmentFactory.Create(visitorMock.Object);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(visitorMock.Object.Id, result.Id);
        Assert.Equal(visitorMock.Object.DeviceId, result.DeviceId);
        Assert.Equal(visitorMock.Object.CollaboratorId, result.CollaboratorId);
        Assert.Equal(visitorMock.Object.PeriodDate, result.PeriodDate);
    }


}