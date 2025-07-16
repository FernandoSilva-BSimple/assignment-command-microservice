using Domain.Factory.AssignmentTempFactory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Domain.Visitors;
using Moq;

namespace Domain.Tests.AssignmentTempDomainTests
{
    public class AssignmentTempFactoryTests
    {
        [Fact]
        public void WhenCreatingAssignmentTempWithValidFieldsAndId_ThenAssignmentIsCreated()
        {
            // Arrange
            var collabRepositoryMock = new Mock<ICollaboratorRepository>();

            var assignmentFactory = new AssignmentTempFactory(collabRepositoryMock.Object);

            // Act
            var assignment = assignmentFactory.Create(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<PeriodDate>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

            // Assert
            Assert.NotNull(assignment);
        }
        [Fact]
        public async Task WhenCreatingAssignmentTempWithValidFields_ThenAssignmentTempIsCreated()
        {
            // Arrange
            var collaboratorId = Guid.NewGuid();
            var collabPeriodStart = new DateTime(2025, 7, 1);
            var collabPeriodEnd = new DateTime(2025, 7, 31);
            var collabPeriodDateTime = new PeriodDateTime(collabPeriodStart, collabPeriodEnd);
            var assignmentPeriod = new PeriodDate(new DateOnly(2025, 7, 10), new DateOnly(2025, 7, 20));

            var collaboratorMock = new Mock<ICollaborator>();
            collaboratorMock.Setup(c => c.Id).Returns(collaboratorId);
            collaboratorMock.Setup(c => c.PeriodDateTime).Returns(collabPeriodDateTime);

            var collaboratorRepositoryMock = new Mock<ICollaboratorRepository>();
            collaboratorRepositoryMock.Setup(cr => cr.GetById(collaboratorId))
                                      .ReturnsAsync(collaboratorMock.Object);

            var factory = new AssignmentTempFactory(collaboratorRepositoryMock.Object);

            // Act
            var result = await factory.Create(collaboratorId, assignmentPeriod, "Laptop", "Dell", "Latitude", "SN123");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(collaboratorId, result.CollaboratorId);
            Assert.Equal(assignmentPeriod, result.PeriodDate);
            Assert.Equal("Laptop", result.DeviceDescription);
            Assert.Equal("Dell", result.DeviceBrand);
            Assert.Equal("Latitude", result.DeviceModel);
            Assert.Equal("SN123", result.DeviceSerialNumber);
        }

        [Fact]
        public async Task WhenCreatingAssignmentTempWithNonExistingCollaborator_ThenShouldThrowException()
        {
            // Arrange
            var collaboratorId = Guid.NewGuid();
            var assignmentPeriod = new PeriodDate(new DateOnly(2025, 7, 10), new DateOnly(2025, 7, 20));

            var collaboratorRepositoryMock = new Mock<ICollaboratorRepository>();
            collaboratorRepositoryMock.Setup(r => r.GetById(collaboratorId)).ReturnsAsync((ICollaborator)null!);

            var factory = new AssignmentTempFactory(collaboratorRepositoryMock.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() =>
                factory.Create(collaboratorId, assignmentPeriod, "Laptop", "Dell", "Latitude", "SN123"));

            Assert.Equal("Collaborator not found", exception.Message);
        }

        [Fact]
        public async Task WhenAssignmentTempPeriodIsOutsideCollaboratorPeriod_ThenShouldThrowException()
        {
            // Arrange
            var collaboratorId = Guid.NewGuid();
            var collabPeriodStart = new DateTime(2025, 7, 1);
            var collabPeriodEnd = new DateTime(2025, 7, 5);
            var collabPeriodDateTime = new PeriodDateTime(collabPeriodStart, collabPeriodEnd);
            var assignmentPeriod = new PeriodDate(new DateOnly(2025, 7, 10), new DateOnly(2025, 7, 20));

            var collaboratorMock = new Mock<ICollaborator>();
            collaboratorMock.Setup(c => c.Id).Returns(collaboratorId);
            collaboratorMock.Setup(c => c.PeriodDateTime).Returns(collabPeriodDateTime);

            var collaboratorRepositoryMock = new Mock<ICollaboratorRepository>();
            collaboratorRepositoryMock.Setup(r => r.GetById(collaboratorId)).ReturnsAsync(collaboratorMock.Object);

            var factory = new AssignmentTempFactory(collaboratorRepositoryMock.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() =>
                factory.Create(collaboratorId, assignmentPeriod, "Laptop", "Dell", "Latitude", "SN123"));

            Assert.Equal("Assignment period must be within the collaborator's active period", exception.Message);
        }

        [Fact]
        public void WhenCreatingAssignmentTempFromVisitor_ThenAssignmentTempIsCreated()
        {
            // Arrange
            var visitorMock = new Mock<IAssignmentTempVisitor>();
            visitorMock.Setup(v => v.Id).Returns(It.IsAny<Guid>());
            visitorMock.Setup(v => v.CollaboratorId).Returns(It.IsAny<Guid>());
            visitorMock.Setup(v => v.PeriodDate).Returns(new PeriodDate(new DateOnly(2025, 7, 10), new DateOnly(2025, 7, 20)));
            visitorMock.Setup(v => v.DeviceDescription).Returns(It.IsAny<string>());
            visitorMock.Setup(v => v.DeviceBrand).Returns(It.IsAny<string>());
            visitorMock.Setup(v => v.DeviceModel).Returns(It.IsAny<string>());
            visitorMock.Setup(v => v.DeviceSerialNumber).Returns(It.IsAny<string>());

            var factory = new AssignmentTempFactory();

            // Act
            var result = factory.Create(visitorMock.Object);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(visitorMock.Object.Id, result.Id);
            Assert.Equal(visitorMock.Object.CollaboratorId, result.CollaboratorId);
            Assert.Equal(visitorMock.Object.PeriodDate, result.PeriodDate);
            Assert.Equal(visitorMock.Object.DeviceDescription, result.DeviceDescription);
            Assert.Equal(visitorMock.Object.DeviceBrand, result.DeviceBrand);
            Assert.Equal(visitorMock.Object.DeviceModel, result.DeviceModel);
            Assert.Equal(visitorMock.Object.DeviceSerialNumber, result.DeviceSerialNumber);
        }

    }
}