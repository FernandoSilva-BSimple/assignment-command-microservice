using Domain.Interfaces;
using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;

namespace Infrastructure.Tests.AssignmentTempRepositoryTests;

public class AssignmentTempRepositoryCreateAssignmentTempAsyncTests : RepositoryTestBase
{
    [Fact]
    public async Task WhenCreatingAssignmentTemp_ThenItIsCreatedAndReturned()
    {
        //arrange
        var id = Guid.NewGuid();
        var collaboratorId = Guid.NewGuid();
        var period = new PeriodDate(new DateOnly(2025, 7, 1), new DateOnly(2025, 7, 31));
        var deviceDescription = "Test Device Description";
        var deviceBrand = "Test Device Brand";
        var deviceModel = "Test Device Model";
        var deviceSerialNumber = "1234567890";

        var assignmentTempMock = new Mock<IAssignmentTemp>();

        assignmentTempMock.Setup(a => a.CollaboratorId).Returns(collaboratorId);
        assignmentTempMock.Setup(a => a.PeriodDate).Returns(period);
        assignmentTempMock.Setup(a => a.DeviceDescription).Returns(deviceDescription);
        assignmentTempMock.Setup(a => a.DeviceBrand).Returns(deviceBrand);
        assignmentTempMock.Setup(a => a.DeviceModel).Returns(deviceModel);
        assignmentTempMock.Setup(a => a.DeviceSerialNumber).Returns(deviceSerialNumber);

        var assignmentTempDM = new AssignmentTempDataModel
        {
            Id = id,
            CollaboratorId = collaboratorId,
            PeriodDate = period,
            DeviceDescription = deviceDescription,
            DeviceBrand = deviceBrand,
            DeviceModel = deviceModel,
            DeviceSerialNumber = deviceSerialNumber
        };

        _mapper.Setup(m => m.Map<AssignmentTempDataModel>(It.IsAny<IAssignmentTemp>())).Returns(assignmentTempDM);
        _mapper.Setup(m => m.Map<IAssignmentTemp>(It.IsAny<AssignmentTempDataModel>())).Returns(assignmentTempMock.Object);

        var repository = new AssignmentTempRepository(_mapper.Object, context);

        //act
        var result = await repository.CreateAssignmentTempAsync(assignmentTempMock.Object);

        //assert
        Assert.NotNull(result);
        Assert.Equal(assignmentTempMock.Object, result);

        var inDb = await context.AssignmentsTemp.FindAsync(id);
        Assert.NotNull(inDb);
        Assert.Equal(assignmentTempMock.Object.CollaboratorId, inDb.CollaboratorId);
        Assert.Equal(assignmentTempMock.Object.PeriodDate, inDb.PeriodDate);
        Assert.Equal(assignmentTempMock.Object.DeviceDescription, inDb.DeviceDescription);
        Assert.Equal(assignmentTempMock.Object.DeviceBrand, inDb.DeviceBrand);
        Assert.Equal(assignmentTempMock.Object.DeviceModel, inDb.DeviceModel);
        Assert.Equal(assignmentTempMock.Object.DeviceSerialNumber, inDb.DeviceSerialNumber);
    }
}