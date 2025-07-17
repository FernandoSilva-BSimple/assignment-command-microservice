using Application.IPublishers;
using Application.Services;
using Domain.Factory.AssignmentFactory;
using Domain.Interfaces;
using Domain.IRepository;
using Moq;

namespace Application.Tests.AssignmentServiceTests;

public class AssignmentServiceCreateWithoutPublishTests
{
    [Fact]
    public async Task WhenCreateAssignmentWithoutPublish_ThenShouldCreateAssignmentWithoutPublishingIt()
    {
        // Arrange
        var assignmentRepo = new Mock<IAssignmentRepository>();

        var assigmentMock = new Mock<IAssignment>();

        var service = new AssignmentService(assignmentRepo.Object, Mock.Of<IAssignmentFactory>(), Mock.Of<IMessagePublisher>(), Mock.Of<IDeviceRepository>(), Mock.Of<ICollaboratorRepository>());

        assignmentRepo.Setup(r => r.CreateAssignmentAsync(assigmentMock.Object))
              .ReturnsAsync(assigmentMock.Object);


        // Act
        var result = await service.CreateWithoutPublish(assigmentMock.Object);

        // Assert
        Assert.Equal(assigmentMock.Object, result);
        assignmentRepo.Verify(r => r.CreateAssignmentAsync(assigmentMock.Object), Times.Once);
    }
}