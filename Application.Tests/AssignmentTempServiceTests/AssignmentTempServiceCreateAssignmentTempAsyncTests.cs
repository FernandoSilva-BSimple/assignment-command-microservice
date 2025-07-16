using Application.DTO.AssignmentTemp;
using Application.IPublishers;
using Application.Services;
using Domain.Factory.AssignmentTempFactory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Moq;

namespace Application.Tests.AssignmentTempServiceTests;

public class CreateAssignmentTempTests
{
    [Fact]
    public async Task CreateAssignmentTempAsync_ShouldCallFactoryAndRepository_WithCorrectData()
    {
        // Arrange
        var collaboratorId = Guid.NewGuid();
        var period = new PeriodDate(new DateOnly(2025, 7, 20), new DateOnly(2025, 7, 30));
        var description = "Laptop";
        var brand = "Dell";
        var model = "XPS 15";
        var serial = "123ABC456";

        var dto = new CreateAssignmentTempDTO(collaboratorId, period, description, brand, model, serial);

        var assignmentTempMock = new Mock<IAssignmentTemp>();

        var factoryMock = new Mock<IAssignmentTempFactory>();
        factoryMock.Setup(f => f.Create(collaboratorId, period, description, brand, model, serial))
                   .ReturnsAsync(assignmentTempMock.Object);

        var repoMock = new Mock<IAssignmentTempTempRepository>();

        repoMock.Setup(r => r.CreateAssignmentTempAsync(assignmentTempMock.Object))
         .ReturnsAsync(assignmentTempMock.Object);


        var publisherMock = new Mock<IMessagePublisher>();

        var service = new AssignmentTempService(repoMock.Object, factoryMock.Object, publisherMock.Object);

        // Act
        await service.CreateAssignmentTempAsync(dto);

        // Assert
        factoryMock.Verify(f => f.Create(collaboratorId, period, description, brand, model, serial), Times.Once);
        repoMock.Verify(r => r.CreateAssignmentTempAsync(assignmentTempMock.Object), Times.Once);
    }
}
