using Domain.Models;

namespace Domain.Tests.DeviceDomainTests;

public class DeviceConstructorTests
{

    [Fact]
    public void WhenCreatingDeviceWithId_ThenDeviceIsCreated()
    {
        //arrange
        Guid id = Guid.NewGuid(); ;

        //act & assert
        new Device(id);

    }
}