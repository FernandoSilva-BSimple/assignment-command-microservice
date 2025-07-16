using System;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;

namespace Infrastructure.Tests.CollaboratorRepositoryTests;

public class CollaboratorRepositoryGetByIdAsyncTests : RepositoryTestBase
{
    [Fact]
    public async Task WhenCollaboratorExists_ThenGetByIdReturnsCollaborator()
    {
        // Arrange
        var collaboratorId = Guid.NewGuid();
        var collaboratorDM = new CollaboratorDataModel
        {
            Id = collaboratorId,
            PeriodDateTime = new PeriodDateTime
            (
                new DateTime(2025, 7, 1),
                new DateTime(2025, 7, 31))
        };

        context.Collaborators.Add(collaboratorDM);
        await context.SaveChangesAsync();

        var collaboratorMock = new Mock<ICollaborator>();
        _mapper.Setup(m => m.Map<ICollaborator>(It.IsAny<CollaboratorDataModel>()))
               .Returns(collaboratorMock.Object);

        var repository = new CollaboratorRepository(_mapper.Object, context);

        // Act
        var result = await repository.GetByIdAsync(collaboratorId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(collaboratorMock.Object, result);
    }

    [Fact]
    public async Task WhenCollaboratorDoesNotExist_ThenGetByIdReturnsNull()
    {
        // Arrange
        var repository = new CollaboratorRepository(_mapper.Object, context);
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await repository.GetByIdAsync(nonExistentId);

        // Assert
        Assert.Null(result);
    }
}

