using Domain.Interfaces;
using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;

namespace Infrastructure.Tests.CollaboratorRepositoryTests;

public class CollaboratorRepositoryAddCollaboratorAsyncTests : RepositoryTestBase
{
    [Fact]
    public async Task WhenCreatingCollaborator_ThenItIsCreatedAndReturned()
    {
        //Arrange
        var id = Guid.NewGuid();
        var period = new PeriodDateTime(new DateTime(2025, 7, 1), new DateTime(2025, 7, 31));

        var collaboratorMock = new Mock<ICollaborator>();

        collaboratorMock.Setup(c => c.Id).Returns(id);
        collaboratorMock.Setup(c => c.PeriodDateTime).Returns(period);

        var collaboratorDM = new CollaboratorDataModel
        {
            Id = id,
            PeriodDateTime = period
        };

        _mapper.Setup(m => m.Map<CollaboratorDataModel>(It.IsAny<ICollaborator>()))
               .Returns(collaboratorDM);

        _mapper.Setup(m => m.Map<ICollaborator>(It.IsAny<CollaboratorDataModel>()))
            .Returns(collaboratorMock.Object);

        var repository = new CollaboratorRepository(_mapper.Object, context);

        //Act
        var result = await repository.AddCollaboratorAsync(collaboratorMock.Object);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(collaboratorMock.Object, result);
    }

}
