using Application.Services;
using Domain.Factory.DeviceFactory;
using Domain.Interfaces;
using Domain.IRepository;
using Moq;

namespace Application.Tests.DeviceServiceTests
{
    public class DeviceServiceAddConsumedDeviceAsync
    {
        [Fact]
        public async Task AddConsumedDeviceAsync_AddsDeviceToRepository()
        {
            // Arrange
            var deviceRepositoryMock = new Mock<IDeviceRepository>();
            var deviceFactoryMock = new Mock<IDeviceFactory>();
            var deviceService = new DeviceService(deviceRepositoryMock.Object, deviceFactoryMock.Object);

            // Act
            var result = await deviceService.AddConsumedDeviceAsync(Guid.NewGuid());

            // Assert
            deviceRepositoryMock.Verify(repo => repo.AddDeviceAsync(It.IsAny<IDevice>()), Times.Once);
        }
    }
}