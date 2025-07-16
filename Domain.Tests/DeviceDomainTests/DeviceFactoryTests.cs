using Domain.Factory.DeviceFactory;
using Domain.Models;
using Domain.Visitors;
using Moq;

namespace Domain.Tests.DeviceDomainTests;

public class DeviceFactoryTests
{

    [Fact]
    public void WhenCreatingDevice_ThenDeviceIsCreated()
    {
        //arrange
        var deviceFactory = new DeviceFactory();

        //act
        var device = deviceFactory.Create(Guid.NewGuid());

        //assert
        Assert.NotNull(device);
    }

    [Fact]
    public void WhenCreatingDeviceFromVisitor_ThenDeviceIsCreated()
    {
        //arrange
        var deviceFactory = new DeviceFactory();
        var deviceVisitor = new Mock<IDeviceVisitor>();

        //act
        var device = deviceFactory.Create(deviceVisitor.Object);

        //assert
        Assert.NotNull(device);
    }
}