using Application.DTO.Assignment;
using Application.DTO.AssignmentTemp;
using Application.IPublishers;
using Application.Services;
using Domain.Commands;
using Domain.Factory.AssignmentTempFactory;
using Domain.IRepository;
using Domain.Models;
using Moq;

namespace Application.Tests.AssignmentTempServiceTests;

public class AssignmentTempServiceStartSagaTests
{
    [Fact]
    public async Task StartSagaAsync_ShouldSendCorrectCommand()
    {
        // Arrange
        var dto = new CreateAssignmentAndDeviceDTO(
            Guid.NewGuid(),
            new PeriodDate(new DateOnly(2025, 8, 1), new DateOnly(2025, 8, 15)),
            "Laptop",
            "Dell",
            "Latitude",
            "SN123"
        );

        var publisherMock = new Mock<IMessagePublisher>();
        var repoMock = new Mock<IAssignmentTempTempRepository>();
        var factoryMock = new Mock<IAssignmentTempFactory>();

        var service = new AssignmentTempService(repoMock.Object, factoryMock.Object, publisherMock.Object);

        // Act
        await service.StartSagaAsync(dto);

        // Assert
        publisherMock.Verify(p => p.SendCreateAssignmentSagaCommandAsync(
            It.Is<CreateRequestedAssignmentCommand>(cmd =>
                cmd.CollaboratorId == dto.CollaboratorId &&
                cmd.StartDate == dto.PeriodDate.InitDate &&
                cmd.EndDate == dto.PeriodDate.FinalDate &&
                cmd.DeviceDescription == dto.DeviceDescription &&
                cmd.DeviceBrand == dto.DeviceBrand &&
                cmd.DeviceModel == dto.DeviceModel &&
                cmd.DeviceSerialNumber == dto.DeviceSerialNumber
            )), Times.Once);
    }
}
