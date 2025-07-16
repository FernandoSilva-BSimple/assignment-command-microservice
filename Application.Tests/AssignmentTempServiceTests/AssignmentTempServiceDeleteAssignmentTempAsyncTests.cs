using Application.IPublishers;
using Application.Services;
using Domain.Factory.AssignmentTempFactory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Moq;

namespace Application.Tests.AssignmentTempServiceTests;

public class AssignmentTempServiceDeleteTests
{
    [Fact]
    public async Task DeleteAssignmentTempAsync_ShouldRemove_WhenExists()
    {
        // Arrange
        var id = Guid.NewGuid();

        var assignmentTempMock = new Mock<IAssignmentTemp>();
        assignmentTempMock.Setup(a => a.Id).Returns(id);

        var repoMock = new Mock<IAssignmentTempTempRepository>();
        repoMock.Setup(r => r.GetAssignmentTempByIdAsync(id)).ReturnsAsync(assignmentTempMock.Object);
        repoMock.Setup(r => r.RemoveAssignmentTempAsync(assignmentTempMock.Object)).Returns(Task.CompletedTask).Verifiable();

        var service = new AssignmentTempService(repoMock.Object, Mock.Of<IAssignmentTempFactory>(), Mock.Of<IMessagePublisher>());

        // Act
        await service.DeleteAssignmentTempAsync(id);

        // Assert
        repoMock.Verify(r => r.RemoveAssignmentTempAsync(assignmentTempMock.Object), Times.Once);
    }

    [Fact]
    public async Task DeleteAssignmentTempAsync_ShouldThrow_WhenNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();

        var repoMock = new Mock<IAssignmentTempTempRepository>();
        repoMock.Setup(r => r.GetAssignmentTempByIdAsync(id)).ReturnsAsync((IAssignmentTemp?)null);

        var service = new AssignmentTempService(repoMock.Object, Mock.Of<IAssignmentTempFactory>(), Mock.Of<IMessagePublisher>());

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.DeleteAssignmentTempAsync(id));
        Assert.Equal("AssignmentTemp not found", ex.Message);
    }
}
