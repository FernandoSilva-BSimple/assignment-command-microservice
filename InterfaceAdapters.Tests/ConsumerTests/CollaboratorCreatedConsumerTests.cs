using Application.Interfaces;
using Contracts.Messages;
using Domain.Models;
using InterfaceAdapters.Consumers;
using MassTransit;
using Moq;
using Xunit;

namespace InterfaceAdapters.Tests.ConsumerTests
{
    public class CollaboratorConsumerTests
    {
        [Fact]
        public async Task Consume_ShouldCallAddCollaboratorReferenceAsync_WithCorrectData()
        {
            // Arrange
            var serviceDouble = new Mock<ICollaboratorService>();
            var consumer = new CollaboratorCreatedConsumer(serviceDouble.Object);

            var userId = Guid.NewGuid();
            var start = DateTime.Now;
            var end = start.AddYears(1);

            var message = new CollaboratorCreatedMessage(Guid.NewGuid(), userId, start, end);
            var context = Mock.Of<ConsumeContext<CollaboratorCreatedMessage>>(c => c.Message == message);

            // Act
            await consumer.Consume(context);

            // Assert
            serviceDouble.Verify(s =>
                s.AddConsumedCollaboratorAsync(
                    message.Id,
                    It.Is<PeriodDateTime>(p => p._initDate == start && p._finalDate == end)
                    ),
                Times.Once
);
        }
    }
}