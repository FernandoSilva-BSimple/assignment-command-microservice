using Application.DTO.Assignment;
using Application.IPublishers;
using Application.Services;
using Domain.Factory.AssignmentFactory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Moq;

namespace Application.Tests.AssignmentServiceTests
{
    public class AssignmentServiceCreateTests
    {
        [Fact]
        public async Task Create_ShouldCreateAssignmentAndPublishMessage_WhenValidDataIsProvided()
        {
            // Arrange
            var assignmentRepositoryMock = new Mock<IAssignmentRepository>();
            var assignmentFactoryMock = new Mock<IAssignmentFactory>();
            var deviceFactoryMock = new Mock<IDeviceRepository>();
            var collaboratorRepositoryMock = new Mock<ICollaboratorRepository>();
            var messagePublisherMock = new Mock<IMessagePublisher>();

            var assignmentId = Guid.NewGuid();
            var deviceId = Guid.NewGuid();
            var collaboratorId = Guid.NewGuid();
            var assignmentPeriod = new PeriodDate(new DateOnly(2025, 7, 10), new DateOnly(2025, 7, 20));

            var assignmentMock = new Mock<IAssignment>();
            assignmentMock.Setup(a => a.Id).Returns(assignmentId);
            assignmentMock.Setup(a => a.DeviceId).Returns(deviceId);
            assignmentMock.Setup(a => a.CollaboratorId).Returns(collaboratorId);
            assignmentMock.Setup(a => a.PeriodDate).Returns(assignmentPeriod);


            assignmentFactoryMock.Setup(factory => factory.Create(deviceId, collaboratorId, assignmentPeriod)).ReturnsAsync(assignmentMock.Object);

            assignmentRepositoryMock.Setup(repo => repo.CreateAssignmentAsync(assignmentMock.Object)).ReturnsAsync(assignmentMock.Object);

            var service = new AssignmentService(assignmentRepositoryMock.Object, assignmentFactoryMock.Object, messagePublisherMock.Object, deviceFactoryMock.Object, collaboratorRepositoryMock.Object);
            var createDTO = new CreateAssignmentDTO(deviceId, collaboratorId, assignmentPeriod);

            // Act
            var result = await service.Create(createDTO);

            //Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);

            Assert.Equal(assignmentId, result.Value.Id);
            Assert.Equal(deviceId, result.Value.DeviceId);
            Assert.Equal(collaboratorId, result.Value.CollaboratorId);
            Assert.Equal(assignmentPeriod, result.Value.PeriodDate);

            messagePublisherMock.Verify(publisher => publisher.PublishAssignmentCreatedAsync(assignmentId, deviceId, collaboratorId, assignmentPeriod), Times.Once);
        }

        [Fact]
        public async Task Create_ShouldReturnFailure_WhenFactoryThrowsException()
        {
            // Arrange
            var assignmentRepositoryMock = new Mock<IAssignmentRepository>();
            var assignmentFactoryMock = new Mock<IAssignmentFactory>();
            var deviceFactoryMock = new Mock<IDeviceRepository>();
            var collaboratorRepositoryMock = new Mock<ICollaboratorRepository>();
            var messagePublisherMock = new Mock<IMessagePublisher>();

            var createDTO = new CreateAssignmentDTO(Guid.NewGuid(), Guid.NewGuid(), new PeriodDate(new DateOnly(2025, 7, 10), new DateOnly(2025, 7, 20)));

            var exceptedException = new Exception("Simulated exception");

            assignmentFactoryMock.Setup(factory => factory.Create(createDTO.DeviceId, createDTO.CollaboratorId, createDTO.PeriodDate)).ThrowsAsync(exceptedException);

            var service = new AssignmentService(assignmentRepositoryMock.Object, assignmentFactoryMock.Object, messagePublisherMock.Object, deviceFactoryMock.Object, collaboratorRepositoryMock.Object);

            // Act
            var result = await service.Create(createDTO);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Value);
            Assert.Equal(exceptedException.Message, result.Error.Message);

            messagePublisherMock.Verify(publisher => publisher.PublishAssignmentCreatedAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<PeriodDate>()), Times.Never);
            assignmentRepositoryMock.Verify(repo => repo.CreateAssignmentAsync(It.IsAny<IAssignment>()), Times.Never);
        }
    }
}