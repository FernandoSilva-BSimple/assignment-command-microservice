using System;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;

namespace Infrastructure.Tests.AssignmentTempRepositoryTests;

public class AssignmentTempRepositoryGetByIdAsyncTests : RepositoryTestBase
{
    [Fact]
    public async Task WhenAssignmentTempExists_ThenGetByIdReturnsAssignment()
    {
        // Arrange
        var assigmentTempId = Guid.NewGuid();
        var assignmentTempDM = new AssignmentTempDataModel
        {
            Id = assigmentTempId,
            CollaboratorId = Guid.NewGuid(),
            PeriodDate = new PeriodDate
            (
                new DateOnly(2025, 7, 1),
                new DateOnly(2025, 7, 31)),
            DeviceDescription = "Device Description",
            DeviceBrand = "Device Brand",
            DeviceModel = "Device Model",
            DeviceSerialNumber = "1234567890"
        };

        context.AssignmentsTemp.Add(assignmentTempDM);
        await context.SaveChangesAsync();

        var assignmentTempMock = new Mock<IAssignmentTemp>();
        _mapper.Setup(m => m.Map<IAssignmentTemp>(It.IsAny<AssignmentTempDataModel>()))
               .Returns(assignmentTempMock.Object);

        var repository = new AssignmentTempRepository(_mapper.Object, context);

        // Act
        var result = await repository.GetAssignmentTempByIdAsync(assigmentTempId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(assignmentTempMock.Object, result);
    }

    [Fact]
    public async Task WhenAssignmentTempDoesNotExist_ThenGetByIdReturnsNull()
    {
        // Arrange
        var repository = new AssignmentTempRepository(_mapper.Object, context);
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await repository.GetAssignmentTempByIdAsync(nonExistentId);

        // Assert
        Assert.Null(result);
    }
}

