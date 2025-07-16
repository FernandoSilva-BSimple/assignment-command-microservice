using Domain.Interfaces;
using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;

namespace Infrastructure.Tests.AssignmentRepositoryTests;

public class AssignmentRepositoryUpdateAssignmentAsyncTests : RepositoryTestBase
{
    [Fact]
    public async Task WhenUpdatingAssignment_ThenItIsUpdatedAndReturned()
    {
        // Arrange
        var id = Guid.NewGuid();
        var deviceId = Guid.NewGuid();
        var collaboratorId = Guid.NewGuid();
        var period = new PeriodDate(new DateOnly(2025, 7, 1), new DateOnly(2025, 7, 31));

        var assignmentMock = new Mock<IAssignment>();
        assignmentMock.Setup(a => a.Id).Returns(id);
        assignmentMock.Setup(a => a.DeviceId).Returns(deviceId);
        assignmentMock.Setup(a => a.CollaboratorId).Returns(collaboratorId);
        assignmentMock.Setup(a => a.PeriodDate).Returns(period);

        var existingAssignmentDM = new AssignmentDataModel
        {
            Id = id,
            DeviceId = Guid.NewGuid(),
            CollaboratorId = Guid.NewGuid(),
            PeriodDate = new PeriodDate(new DateOnly(2025, 6, 1), new DateOnly(2025, 6, 30))
        };

        await context.Assignments.AddAsync(existingAssignmentDM);
        await context.SaveChangesAsync();

        _mapper.Setup(m => m.Map<IAssignment>(It.IsAny<AssignmentDataModel>()))
               .Returns(assignmentMock.Object);

        var repository = new AssignmentRepository(_mapper.Object, context);

        // Act
        var result = await repository.UpdateAssignmentAsync(assignmentMock.Object);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(assignmentMock.Object, result);

        var inDb = await context.Assignments.FindAsync(id);
        Assert.NotNull(inDb);
        Assert.Equal(deviceId, inDb.DeviceId);
        Assert.Equal(collaboratorId, inDb.CollaboratorId);
        Assert.Equal(period, inDb.PeriodDate);
    }

    [Fact]
    public async Task WhenUpdatingNonExistingAssignment_ThenReturnsNull()
    {
        // Arrange
        var assignmentMock = new Mock<IAssignment>();
        assignmentMock.Setup(a => a.Id).Returns(Guid.NewGuid());

        var repository = new AssignmentRepository(_mapper.Object, context);

        // Act
        var result = await repository.UpdateAssignmentAsync(assignmentMock.Object);

        // Assert
        Assert.Null(result);
    }
}