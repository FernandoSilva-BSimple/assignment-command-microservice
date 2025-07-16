using Contracts.Messages;
using MassTransit;
using Application.Interfaces;
using Domain.Models;

namespace InterfaceAdapters.Consumers;

public class AssignmentCreatedConsumer : IConsumer<AssignmentCreatedMessage>
{
    private readonly IAssignmentService _assignmentService;

    public AssignmentCreatedConsumer(IAssignmentService assignmentService)
    {
        _assignmentService = assignmentService;
    }

    public async Task Consume(ConsumeContext<AssignmentCreatedMessage> context)
    {
        Console.WriteLine("Estamos a consumir a mensagem de assignmentCreated");

        var period = new PeriodDate(context.Message.StartDate, context.Message.EndDate);

        await _assignmentService.AddConsumedAssignmentAsync(context.Message.AssignmentId, context.Message.CollaboratorId, context.Message.DeviceId, period);
    }
}