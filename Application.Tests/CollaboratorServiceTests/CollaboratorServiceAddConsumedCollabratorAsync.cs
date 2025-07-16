using Application.Services;
using Domain.Factory.CollaboratorFactory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Moq;

namespace Application.Tests.CollaboratorServiceTests
{
    public class CollaboratorServiceAddConsumedCollaboratorAsync
    {
        [Fact]
        public async Task AddConsumedCollaboratorAsync_AddsCollaboratorToRepository()
        {
            // Arrange
            var collaboratorRepositoryMock = new Mock<ICollaboratorRepository>();
            var collaboratorFactoryMock = new Mock<ICollaboratorFactory>();
            var collaboratorService = new CollaboratorService(collaboratorRepositoryMock.Object, collaboratorFactoryMock.Object);

            // Act
            var result = await collaboratorService.AddConsumedCollaboratorAsync(Guid.NewGuid(), new PeriodDateTime());

            // Assert
            collaboratorRepositoryMock.Verify(repo => repo.AddCollaboratorAsync(It.IsAny<ICollaborator>()), Times.Once);
        }
    }
}