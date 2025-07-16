using System;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;

namespace Infrastructure.Tests.AssignmentRepositoryTests;

public class AssignmentRepositoryGetByIdAsyncTests : RepositoryTestBase
{
    [Fact]
    public async Task WhenAssignmentExists_ThenGetByIdReturnsAssignment()
    {
        // Arrange
        var assigmentId = Guid.NewGuid();
        var assignmentDM = new AssignmentDataModel
        {
            Id = assigmentId,
            CollaboratorId = Guid.NewGuid(),
            DeviceId = Guid.NewGuid(),
            PeriodDate = new PeriodDate
            (
                new DateOnly(2025, 7, 1),
                new DateOnly(2025, 7, 31))
        };

        context.Assignments.Add(assignmentDM);
        await context.SaveChangesAsync();

        var assignmentMock = new Mock<IAssignment>();
        _mapper.Setup(m => m.Map<IAssignment>(It.IsAny<AssignmentDataModel>()))
               .Returns(assignmentMock.Object);

        var repository = new AssignmentRepository(_mapper.Object, context);

        // Act
        var result = await repository.GetAssignmentByIdAsync(assigmentId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(assignmentMock.Object, result);
    }

    [Fact]
    public async Task WhenAssignmentDoesNotExist_ThenGetByIdReturnsNull()
    {
        // Arrange
        var repository = new AssignmentRepository(_mapper.Object, context);
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await repository.GetAssignmentByIdAsync(nonExistentId);

        // Assert
        Assert.Null(result);
    }
}

