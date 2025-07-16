using Application.IPublishers;
using Contracts.Commands;
using Contracts.Messages;
using Domain.Models;
using MassTransit;

namespace InterfaceAdapters.Publisher;

public class MassTransitPublisher : IMessagePublisher
{

    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ISendEndpointProvider _sendEndpointProvider;

    public MassTransitPublisher(IPublishEndpoint publishEndpoint, ISendEndpointProvider sendEndpointProvider)
    {
        _publishEndpoint = publishEndpoint;
        _sendEndpointProvider = sendEndpointProvider;
    }

    public Task PublishAssignmentCreatedAsync(Guid id, Guid deviceId, Guid collaboratorId, PeriodDate periodDate)
    {
        var eventMessage = new AssignmentCreatedMessage(id, deviceId, collaboratorId, periodDate.InitDate, periodDate.FinalDate);
        return _publishEndpoint.Publish(eventMessage);
    }

    public Task PublishAssignmentUpdatedAsync(Guid id, Guid deviceId, Guid collaboratorId, PeriodDate periodDate)
    {
        var eventMessage = new AssignmentUpdatedMessage(id, deviceId, collaboratorId, periodDate.InitDate, periodDate.FinalDate);
        return _publishEndpoint.Publish(eventMessage);
    }

    public async Task SendCreateAssignmentSagaCommandAsync(CreateRequestedAssignmentCommand command)
    {
        var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:assignments-cmd-saga-{InstanceInfo.InstanceId}"));
        await endpoint.Send(command);
        Console.WriteLine("CreateAssignmentSagaCommand was SENT!");
    }
}