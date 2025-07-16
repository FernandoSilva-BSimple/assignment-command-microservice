using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Repositories;

namespace Infrastructure.Tests.AssignmentRepositoryTests;

public class AssignmentRepositoryExistsWithDeviceAndOverlappingPeriodAsyncTests : RepositoryTestBase
{
    [Fact]
    public async Task WhenAssignmentExistsWithDeviceAndOverlappingPeriod_ThenReturnsTrue()
    {
        // Arrange
        var deviceId = Guid.NewGuid();
        var assignmentDM = new AssignmentDataModel
        {
            Id = Guid.NewGuid(),
            CollaboratorId = Guid.NewGuid(),
            DeviceId = deviceId,
            PeriodDate = new PeriodDate
            (
                new DateOnly(2025, 7, 1),
                new DateOnly(2025, 7, 31))
        };

        context.Assignments.Add(assignmentDM);
        await context.SaveChangesAsync();

        var repository = new AssignmentRepository(_mapper.Object, context);

        var periodDate = new PeriodDate(new DateOnly(2025, 7, 1), new DateOnly(2025, 7, 31));

        // Act
        var result = await repository.ExistsWithDeviceAndOverlappingPeriod(deviceId, periodDate);

        //Asert
        Assert.True(result);
    }

    [Fact]
    public async Task WhenAssignmentDoesNotExistWithDeviceAndOverlappingPeriod_ThenReturnsFalse()
    {
        // Arrange
        var deviceId = Guid.NewGuid();
        var assignmentDM = new AssignmentDataModel
        {
            Id = Guid.NewGuid(),
            CollaboratorId = Guid.NewGuid(),
            DeviceId = deviceId,
            PeriodDate = new PeriodDate
            (
                new DateOnly(2025, 7, 1),
                new DateOnly(2025, 7, 31))
        };

        context.Assignments.Add(assignmentDM);
        await context.SaveChangesAsync();

        var repository = new AssignmentRepository(_mapper.Object, context);

        var periodDate = new PeriodDate(new DateOnly(2025, 8, 1), new DateOnly(2025, 8, 31));

        // Act
        var result = await repository.ExistsWithDeviceAndOverlappingPeriod(deviceId, periodDate);

        //Asert
        Assert.False(result);
    }

    [Fact]
    public async Task WhenAssignmentDoesNotExist_ThenReturnsFalse()
    {
        // Arrange
        var deviceId = Guid.NewGuid();
        var assignmentDM = new AssignmentDataModel
        {
            Id = Guid.NewGuid(),
            CollaboratorId = Guid.NewGuid(),
            DeviceId = deviceId,
            PeriodDate = new PeriodDate
            (
                new DateOnly(2025, 7, 1),
                new DateOnly(2025, 7, 31))
        };

        context.Assignments.Add(assignmentDM);
        await context.SaveChangesAsync();

        var repository = new AssignmentRepository(_mapper.Object, context);

        var periodDate = new PeriodDate(new DateOnly(2025, 7, 1), new DateOnly(2025, 7, 31));
        var otherDeviceId = Guid.NewGuid();

        // Act
        var result = await repository.ExistsWithDeviceAndOverlappingPeriod(otherDeviceId, periodDate);

        //Asert
        Assert.False(result);
    }
}