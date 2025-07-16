using Contracts.Commands;
using Domain.Models;

namespace Application.IPublishers
{
    public interface IMessagePublisher
    {
        Task PublishAssignmentCreatedAsync(Guid id, Guid deviceId, Guid collaboratorId, PeriodDate periodDate);
        Task PublishAssignmentUpdatedAsync(Guid id, Guid deviceId, Guid collaboratorId, PeriodDate periodDate);
        Task SendCreateAssignmentSagaCommandAsync(CreateRequestedAssignmentCommand command);
    }
}