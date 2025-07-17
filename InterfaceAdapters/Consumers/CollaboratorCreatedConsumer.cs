using Application.Interfaces;
using Domain.Messages;
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
        return _collaboratorService.AddConsumedCollaboratorAsync(collabId, context.Message.PeriodDateTime);
    }
}