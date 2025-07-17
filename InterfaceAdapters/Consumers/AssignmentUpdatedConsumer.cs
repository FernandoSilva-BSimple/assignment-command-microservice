using Application.Interfaces;
using Domain.Interfaces;
using Domain.Messages;
using Domain.Models;
using MassTransit;

namespace InterfaceAdapters.Consumers;

public class AssignmentUpdatedConsumer : IConsumer<AssignmentUpdatedMessage>
{
    private readonly IAssignmentService _assignmentService;

    public AssignmentUpdatedConsumer(IAssignmentService assignmentService)
    {
        _assignmentService = assignmentService;
    }

    public async Task Consume(ConsumeContext<AssignmentUpdatedMessage> context)
    {
        var period = new PeriodDate(context.Message.StartDate, context.Message.EndDate);

        await _assignmentService.UpdateConsumedAssignmentAsync(context.Message.AssignmentId, context.Message.CollaboratorId, context.Message.DeviceId, period);
    }
}