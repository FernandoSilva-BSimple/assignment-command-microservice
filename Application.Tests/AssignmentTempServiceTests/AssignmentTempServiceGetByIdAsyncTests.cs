using Application.DTO.AssignmentTemp;
using Application.IPublishers;
using Application.Services;
using Domain.Factory.AssignmentTempFactory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Moq;

namespace Application.Tests.AssignmentTempServiceTests;

public class AssignmentTempServiceGetByIdTests
{
    [Fact]
    public async Task GetByIdAsync_ShouldReturnDTO_WhenAssignmentTempExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var collabId = Guid.NewGuid();
        var period = new PeriodDate(new DateOnly(2025, 7, 1), new DateOnly(2025, 7, 10));
        var description = "Laptop";
        var brand = "Dell";
        var model = "Latitude";
        var serial = "123ABC";

        var assignmentTempMock = new Mock<IAssignmentTemp>();
        assignmentTempMock.SetupGet(a => a.Id).Returns(id);
        assignmentTempMock.SetupGet(a => a.CollaboratorId).Returns(collabId);
        assignmentTempMock.SetupGet(a => a.PeriodDate).Returns(period);
        assignmentTempMock.SetupGet(a => a.DeviceDescription).Returns(description);
        assignmentTempMock.SetupGet(a => a.DeviceBrand).Returns(brand);
        assignmentTempMock.SetupGet(a => a.DeviceModel).Returns(model);
        assignmentTempMock.SetupGet(a => a.DeviceSerialNumber).Returns(serial);

        var repoMock = new Mock<IAssignmentTempTempRepository>();
        repoMock.Setup(r => r.GetAssignmentTempByIdAsync(id)).ReturnsAsync(assignmentTempMock.Object);

        var service = new AssignmentTempService(repoMock.Object, Mock.Of<IAssignmentTempFactory>(), Mock.Of<IMessagePublisher>());

        // Act
        var result = await service.GetByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result!.Id);
        Assert.Equal(collabId, result.CollaboratorId);
        Assert.Equal(period, result.PeriodDate);
        Assert.Equal(description, result.DeviceDescription);
        Assert.Equal(brand, result.DeviceBrand);
        Assert.Equal(model, result.DeviceModel);
        Assert.Equal(serial, result.DeviceSerialNumber);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenAssignmentTempDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();

        var repoMock = new Mock<IAssignmentTempTempRepository>();
        repoMock.Setup(r => r.GetAssignmentTempByIdAsync(id)).ReturnsAsync((IAssignmentTemp?)null);

        var service = new AssignmentTempService(repoMock.Object, Mock.Of<IAssignmentTempFactory>(), Mock.Of<IMessagePublisher>());

        // Act
        var result = await service.GetByIdAsync(id);

        // Assert
        Assert.Null(result);
    }
}
