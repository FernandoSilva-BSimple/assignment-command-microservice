using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Infrastructure.Tests.DeviceRepositoryTests;

public class DeviceRepositoryTests : RepositoryTestBase
{

    public class DeviceRepositoryExistsAsyncTests : RepositoryTestBase
    {
        [Fact]
        public async Task WhenDeviceExists_ThenExistsReturnsTrue()
        {
            // Arrange
            var deviceId = Guid.NewGuid();
            context.Devices.Add(new DeviceDataModel
            {
                Id = deviceId,
            });
            await context.SaveChangesAsync();

            var repository = new DeviceRepository(_mapper.Object, context);

            // Act
            var result = await repository.Exists(deviceId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task WhenDeviceDoesNotExist_ThenExistsReturnsFalse()
        {
            // Arrange
            var repository = new DeviceRepository(_mapper.Object, context);
            var nonExistentId = Guid.NewGuid();

            // Act
            var result = await repository.Exists(nonExistentId);

            // Assert
            Assert.False(result);
        }
    }
}


