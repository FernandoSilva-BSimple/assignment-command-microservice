using Application.IPublishers;
using Contracts.Commands;
using Contracts.Messages;
using Domain.Models;

public class FakePublisherForTests : IMessagePublisher
{
    public Task PublishAssignmentCreatedAsync(Guid id, Guid deviceId, Guid collaboratorId, PeriodDate periodDate)
    {
        return Task.CompletedTask;
    }

    public Task PublishAssignmentUpdatedAsync(Guid id, Guid deviceId, Guid collaboratorId, PeriodDate periodDate)
    {
        return Task.CompletedTask;
    }
    public Task SendCreateAssignmentSagaCommandAsync(CreateRequestedAssignmentCommand command)
    {
        return Task.CompletedTask;
    }
}
