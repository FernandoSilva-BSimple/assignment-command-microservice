using Domain.Interfaces;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;

namespace Infrastructure.Tests.DeviceRepositoryTests;

public class DeviceRepositoryAddDeviceAsyncTests : RepositoryTestBase
{
    [Fact]
    public async Task WhenCreatingDevice_ThenItIsCreatedAndReturned()
    {
        //Arrange
        var id = Guid.NewGuid();

        var deviceMock = new Mock<IDevice>();

        deviceMock.Setup(c => c.Id).Returns(id);

        var deviceDM = new DeviceDataModel
        {
            Id = id,
        };

        _mapper.Setup(m => m.Map<DeviceDataModel>(It.IsAny<IDevice>()))
               .Returns(deviceDM);

        _mapper.Setup(m => m.Map<IDevice>(It.IsAny<DeviceDataModel>()))
            .Returns(deviceMock.Object);

        var repository = new DeviceRepository(_mapper.Object, context);

        //Act
        var result = await repository.AddDeviceAsync(deviceMock.Object);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(deviceMock.Object, result);
    }

}
