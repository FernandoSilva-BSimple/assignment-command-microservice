using Application.Interfaces;
using Contracts.Messages;
using Domain.Models;
using MassTransit;

namespace InterfaceAdapters.Consumers;

public class CollaboratorCreatedConsumer : IConsumer<CollaboratorCreatedMessage>
{
    private readonly ICollaboratorService _collaboratorService;

    public CollaboratorCreatedConsumer(ICollaboratorService collaboratorService)
    {
        _collaboratorService = collaboratorService;
    }

    public Task Consume(ConsumeContext<CollaboratorCreatedMessage> context)
    {
        var collabId = context.Message.Id;
        var collabPeriod = new PeriodDateTime(context.Message.StartDate, context.Message.EndDate);
        return _collaboratorService.AddConsumedCollaboratorAsync(collabId, collabPeriod);
    }
}