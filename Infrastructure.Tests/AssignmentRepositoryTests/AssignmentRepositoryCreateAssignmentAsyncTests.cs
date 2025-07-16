using Domain.Interfaces;
using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;

namespace Infrastructure.Tests.AssignmentRepositoryTests;

public class AssignmentRepositoryCreateAssignmentAsyncTests : RepositoryTestBase
{
    [Fact]
    public async Task WhenCreatingAssignment_ThenItIsCreatedAndReturned()
    {
        //Arrange
        var id = Guid.NewGuid();
        var deviceId = Guid.NewGuid();
        var collaboratorId = Guid.NewGuid();
        var period = new PeriodDate(new DateOnly(2025, 7, 1), new DateOnly(2025, 7, 31));

        var assignmentMock = new Mock<IAssignment>();

        assignmentMock.Setup(a => a.DeviceId).Returns(deviceId);
        assignmentMock.Setup(a => a.CollaboratorId).Returns(collaboratorId);

        var assignmentDM = new AssignmentDataModel
        {
            Id = id,
            DeviceId = deviceId,
            CollaboratorId = collaboratorId,
            PeriodDate = period
        };

        _mapper.Setup(m => m.Map<AssignmentDataModel>(It.IsAny<IAssignment>()))
               .Returns(assignmentDM);

        _mapper.Setup(m => m.Map<IAssignment>(It.IsAny<AssignmentDataModel>()))
            .Returns(assignmentMock.Object);

        var repository = new AssignmentRepository(_mapper.Object, context);

        //act
        var result = await repository.CreateAssignmentAsync(assignmentMock.Object);

        //assert
        Assert.NotNull(result);
        Assert.Equal(assignmentMock.Object, result);

        var inDb = await context.Assignments.FindAsync(id);
        Assert.NotNull(inDb);
        Assert.Equal(assignmentMock.Object.DeviceId, inDb.DeviceId);
        Assert.Equal(assignmentMock.Object.CollaboratorId, inDb.CollaboratorId);
    }
}