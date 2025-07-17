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
    public class AssignmentServiceUpdateTests
    {
        private readonly Guid _assignmentId = Guid.NewGuid();
        private readonly Guid _deviceId = Guid.NewGuid();
        private readonly Guid _collaboratorId = Guid.NewGuid();
        private readonly PeriodDate _validPeriod = new(new DateOnly(2025, 7, 15), new DateOnly(2025, 7, 25));

        private AssignmentService CreateService(
            Mock<IAssignmentRepository> assignmentRepo,
            Mock<IAssignmentFactory> assignmentFactory,
            Mock<IMessagePublisher> publisher,
            Mock<IDeviceRepository> deviceRepo,
            Mock<ICollaboratorRepository> collaboratorRepo
        ) => new(assignmentRepo.Object, assignmentFactory.Object, publisher.Object, deviceRepo.Object, collaboratorRepo.Object);

        [Fact]
        public async Task Update_ShouldUpdateAssignment_WhenValidDataIsProvided()
        {
            // Arrange
            var currentDeviceId = _deviceId;
            var newDeviceId = Guid.NewGuid();
            var currentCollaboratorId = _collaboratorId;
            var newCollaboratorId = Guid.NewGuid();
            var currentPeriod = new PeriodDate(new DateOnly(2025, 7, 1), new DateOnly(2025, 7, 10));

            var assignmentMock = new Mock<IAssignment>();
            assignmentMock.Setup(a => a.Id).Returns(_assignmentId);
            assignmentMock.Setup(a => a.DeviceId).Returns(() => currentDeviceId);
            assignmentMock.Setup(a => a.CollaboratorId).Returns(() => currentCollaboratorId);
            assignmentMock.Setup(a => a.PeriodDate).Returns(() => currentPeriod);

            assignmentMock.Setup(a => a.UpdateDevice(It.IsAny<Guid>())).Callback<Guid>(id => currentDeviceId = id);
            assignmentMock.Setup(a => a.UpdateCollaborator(It.IsAny<Guid>())).Callback<Guid>(id => currentCollaboratorId = id);
            assignmentMock.Setup(a => a.UpdatePeriodDate(It.IsAny<PeriodDate>())).Callback<PeriodDate>(p => currentPeriod = p);

            var assignmentRepo = new Mock<IAssignmentRepository>();
            assignmentRepo.Setup(r => r.GetAssignmentByIdAsync(_assignmentId)).ReturnsAsync(assignmentMock.Object);
            assignmentRepo.Setup(r => r.ExistsWithDeviceAndOverlappingPeriodExcept(It.IsAny<Guid>(), It.IsAny<PeriodDate>(), It.IsAny<Guid>())).ReturnsAsync(false);
            assignmentRepo.Setup(r => r.UpdateAssignmentAsync(It.IsAny<IAssignment>())).ReturnsAsync((IAssignment a) => a);

            var collaborator = new Mock<ICollaborator>();
            collaborator.Setup(c => c.PeriodDateTime).Returns(new PeriodDateTime(new DateTime(2025, 1, 1), new DateTime(2025, 12, 31)));

            var collaboratorRepo = new Mock<ICollaboratorRepository>();
            collaboratorRepo.Setup(r => r.GetByIdAsync(newCollaboratorId)).ReturnsAsync(collaborator.Object);

            var deviceRepo = new Mock<IDeviceRepository>();
            deviceRepo.Setup(r => r.Exists(newDeviceId)).ReturnsAsync(true);

            var service = CreateService(assignmentRepo, new(), new Mock<IMessagePublisher>(), deviceRepo, collaboratorRepo);

            var dto = new UpdateAssignmentDTO(_assignmentId, newDeviceId, newCollaboratorId, _validPeriod);

            // Act
            var result = await service.Update(dto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(newDeviceId, result.Value!.DeviceId);
            Assert.Equal(newCollaboratorId, result.Value.CollaboratorId);
            Assert.Equal(_validPeriod, result.Value.PeriodDate);

            assignmentMock.Verify(a => a.UpdateDevice(newDeviceId), Times.Once);
            assignmentMock.Verify(a => a.UpdateCollaborator(newCollaboratorId), Times.Once);
            assignmentMock.Verify(a => a.UpdatePeriodDate(_validPeriod), Times.Once);
        }
        [Fact]
        public async Task Update_ShouldFail_WhenAssignmentNotFound()
        {
            var assignmentRepo = new Mock<IAssignmentRepository>();
            assignmentRepo.Setup(r => r.GetAssignmentByIdAsync(_assignmentId)).ReturnsAsync((IAssignment?)null);

            var service = CreateService(assignmentRepo, new(), new(), new(), new());
            var dto = new UpdateAssignmentDTO(_assignmentId, _deviceId, _collaboratorId, _validPeriod);

            var result = await service.Update(dto);

            Assert.False(result.IsSuccess);
            Assert.Equal("Assignment not found", result.Error.Message);
        }

        [Fact]
        public async Task Update_ShouldFail_WhenCollaboratorNotFound()
        {
            var assignment = new Mock<IAssignment>();
            assignment.Setup(a => a.Id).Returns(_assignmentId);

            var assignmentRepo = new Mock<IAssignmentRepository>();
            assignmentRepo.Setup(r => r.GetAssignmentByIdAsync(_assignmentId)).ReturnsAsync(assignment.Object);

            var collaboratorRepo = new Mock<ICollaboratorRepository>();
            collaboratorRepo.Setup(r => r.GetByIdAsync(_collaboratorId)).ReturnsAsync((ICollaborator?)null);

            var service = CreateService(assignmentRepo, new(), new(), new(), collaboratorRepo);
            var dto = new UpdateAssignmentDTO(_assignmentId, _deviceId, _collaboratorId, _validPeriod);

            var result = await service.Update(dto);

            Assert.False(result.IsSuccess);
            Assert.Equal("Collaborator not found", result.Error.Message);
        }

        [Fact]
        public async Task Update_ShouldFail_WhenDeviceDoesNotExist()
        {
            var assignment = new Mock<IAssignment>();
            assignment.Setup(a => a.Id).Returns(_assignmentId);

            var assignmentRepo = new Mock<IAssignmentRepository>();
            assignmentRepo.Setup(r => r.GetAssignmentByIdAsync(_assignmentId)).ReturnsAsync(assignment.Object);

            var collaborator = new Mock<ICollaborator>();
            collaborator.Setup(c => c.PeriodDateTime).Returns(new PeriodDateTime(new DateTime(2025, 1, 1), new DateTime(2025, 12, 31)));

            var collaboratorRepo = new Mock<ICollaboratorRepository>();
            collaboratorRepo.Setup(r => r.GetByIdAsync(_collaboratorId)).ReturnsAsync(collaborator.Object);

            var deviceRepo = new Mock<IDeviceRepository>();
            deviceRepo.Setup(r => r.Exists(_deviceId)).ReturnsAsync(false);

            var service = CreateService(assignmentRepo, new(), new(), deviceRepo, collaboratorRepo);
            var dto = new UpdateAssignmentDTO(_assignmentId, _deviceId, _collaboratorId, _validPeriod);

            var result = await service.Update(dto);

            Assert.False(result.IsSuccess);
            Assert.Equal("Device not found", result.Error.Message);
        }

        [Fact]
        public async Task Update_ShouldFail_WhenPeriodOutsideCollaboratorRange()
        {
            var assignment = new Mock<IAssignment>();
            assignment.Setup(a => a.Id).Returns(_assignmentId);

            var assignmentRepo = new Mock<IAssignmentRepository>();
            assignmentRepo.Setup(r => r.GetAssignmentByIdAsync(_assignmentId)).ReturnsAsync(assignment.Object);

            var invalidPeriod = new PeriodDate(new DateOnly(2024, 1, 1), new DateOnly(2026, 1, 1));
            var collaborator = new Mock<ICollaborator>();
            collaborator.Setup(c => c.PeriodDateTime).Returns(new PeriodDateTime(new DateTime(2025, 1, 1), new DateTime(2025, 12, 31)));

            var collaboratorRepo = new Mock<ICollaboratorRepository>();
            collaboratorRepo.Setup(r => r.GetByIdAsync(_collaboratorId)).ReturnsAsync(collaborator.Object);

            var deviceRepo = new Mock<IDeviceRepository>();
            deviceRepo.Setup(r => r.Exists(_deviceId)).ReturnsAsync(true);

            var service = CreateService(assignmentRepo, new(), new(), deviceRepo, collaboratorRepo);
            var dto = new UpdateAssignmentDTO(_assignmentId, _deviceId, _collaboratorId, invalidPeriod);

            var result = await service.Update(dto);

            Assert.False(result.IsSuccess);
            Assert.Equal("Assignment period must be within the collaborator's active period", result.Error.Message);
        }

        [Fact]
        public async Task Update_ShouldFail_WhenAssignmentHasConflict()
        {
            var assignment = new Mock<IAssignment>();
            assignment.Setup(a => a.Id).Returns(_assignmentId);

            var assignmentRepo = new Mock<IAssignmentRepository>();
            assignmentRepo.Setup(r => r.GetAssignmentByIdAsync(_assignmentId)).ReturnsAsync(assignment.Object);
            assignmentRepo.Setup(r => r.ExistsWithDeviceAndOverlappingPeriodExcept(_deviceId, _validPeriod, _assignmentId)).ReturnsAsync(true);

            var collaborator = new Mock<ICollaborator>();
            collaborator.Setup(c => c.PeriodDateTime).Returns(new PeriodDateTime(new DateTime(2025, 1, 1), new DateTime(2025, 12, 31)));

            var collaboratorRepo = new Mock<ICollaboratorRepository>();
            collaboratorRepo.Setup(r => r.GetByIdAsync(_collaboratorId)).ReturnsAsync(collaborator.Object);

            var deviceRepo = new Mock<IDeviceRepository>();
            deviceRepo.Setup(r => r.Exists(_deviceId)).ReturnsAsync(true);

            var service = CreateService(assignmentRepo, new(), new(), deviceRepo, collaboratorRepo);
            var dto = new UpdateAssignmentDTO(_assignmentId, _deviceId, _collaboratorId, _validPeriod);

            var result = await service.Update(dto);

            Assert.False(result.IsSuccess);
            Assert.Equal("Device already has an assignment in this period", result.Error.Message);
        }

        [Fact]
        public async Task Update_ShouldFail_WhenRepositoryUpdateFails()
        {
            var assignmentMock = new Mock<IAssignment>();
            assignmentMock.Setup(a => a.Id).Returns(_assignmentId);

            var assignmentRepo = new Mock<IAssignmentRepository>();
            assignmentRepo.Setup(r => r.GetAssignmentByIdAsync(_assignmentId)).ReturnsAsync(assignmentMock.Object);
            assignmentRepo.Setup(r => r.UpdateAssignmentAsync(It.IsAny<IAssignment>())).ReturnsAsync((IAssignment?)null);

            var collaborator = new Mock<ICollaborator>();
            collaborator.Setup(c => c.PeriodDateTime).Returns(new PeriodDateTime(new DateTime(2025, 1, 1), new DateTime(2025, 12, 31)));

            var collaboratorRepo = new Mock<ICollaboratorRepository>();
            collaboratorRepo.Setup(r => r.GetByIdAsync(_collaboratorId)).ReturnsAsync(collaborator.Object);

            var deviceRepo = new Mock<IDeviceRepository>();
            deviceRepo.Setup(r => r.Exists(_deviceId)).ReturnsAsync(true);

            assignmentRepo.Setup(r => r.ExistsWithDeviceAndOverlappingPeriodExcept(It.IsAny<Guid>(), It.IsAny<PeriodDate>(), It.IsAny<Guid>())).ReturnsAsync(false);

            var service = CreateService(assignmentRepo, new(), new(), deviceRepo, collaboratorRepo);
            var dto = new UpdateAssignmentDTO(_assignmentId, _deviceId, _collaboratorId, _validPeriod);

            var result = await service.Update(dto);

            Assert.False(result.IsSuccess);
            Assert.Equal("Update failed", result.Error.Message);
        }

        [Fact]
        public async Task Update_ShouldFail_WhenExceptionThrown()
        {
            var assignmentRepo = new Mock<IAssignmentRepository>();
            assignmentRepo.Setup(r => r.GetAssignmentByIdAsync(_assignmentId)).ThrowsAsync(new Exception("Boom"));

            var service = CreateService(assignmentRepo, new(), new(), new(), new());
            var dto = new UpdateAssignmentDTO(_assignmentId, _deviceId, _collaboratorId, _validPeriod);

            var result = await service.Update(dto);

            Assert.False(result.IsSuccess);
            Assert.Equal("Boom", result.Error.Message);
        }
    }
}


