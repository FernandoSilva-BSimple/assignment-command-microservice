using Application.DTO.Assignment;
using Application.DTO.AssignmentTemp;
using Application.Interfaces;
using AutoMapper;
using Domain.Commands;
using Domain.Factory.AssignmentFactory;
using Domain.Interfaces;
using Domain.Messages;
using Domain.Models;
using MassTransit;

namespace InterfaceAdapters.Saga;

public class AssignmentSaga : MassTransitStateMachine<AssignmentSagaState>
{
    private readonly IMapper _mapper;

    public State WaitingForDeviceCreation { get; private set; }
    public State Completed { get; private set; }

    public Event<CreateRequestedAssignmentCommand> CreateAssignmentRequested { get; private set; } = default!;
    public Event<DeviceCreatedMessage> DeviceCreated { get; private set; } = default!;

    public AssignmentSaga(IMapper mapper)
    {
        _mapper = mapper;

        InstanceState(x => x.CurrentState);

        Event(() => CreateAssignmentRequested, x => x.CorrelateById(context => context.Message.AssignmentTempId));
        Event(() => DeviceCreated, x =>
        {
            x.CorrelateById(ctx => ctx.Message.CorrelationId ?? Guid.Empty);
            x.OnMissingInstance(m => m.Discard());
        });

        Initially(
            When(CreateAssignmentRequested)
                .ThenAsync(async ctx =>
                {
                    Console.WriteLine("CreateCourtRequested was called!");

                    var provider = ctx.GetPayload<IServiceProvider>();
                    using var scope = provider.CreateScope();

                    var assignmentTempService = scope.ServiceProvider.GetRequiredService<IAssignmentTempService>();

                    await assignmentTempService.CreateAssignmentTempAsync(ctx.Message);

                })
                .Send(new Uri("queue:devices-cmd-saga"), ctx => new CreateDeviceFromAssignmentCommand(
                   ctx.Message.AssignmentTempId,
                   ctx.Message.CollaboratorId,
                   ctx.Message.StartDate,
                   ctx.Message.EndDate,
                   ctx.Message.DeviceDescription,
                   ctx.Message.DeviceBrand,
                   ctx.Message.DeviceModel,
                   ctx.Message.DeviceSerialNumber
                ))
                .TransitionTo(WaitingForDeviceCreation)
        );

        During(WaitingForDeviceCreation,
            When(DeviceCreated)
                .ThenAsync(async ctx =>
                {
                    Console.WriteLine("DeviceCreated was called!");

                    var provider = ctx.GetPayload<IServiceProvider>();
                    using var scope = provider.CreateScope();

                    var assignmentTempService = scope.ServiceProvider.GetRequiredService<IAssignmentTempService>();
                    var assignmentFactory = scope.ServiceProvider.GetRequiredService<IAssignmentFactory>();
                    var assignmentService = scope.ServiceProvider.GetRequiredService<IAssignmentService>();

                    var temp = await assignmentTempService.GetByIdAsync(ctx.Message.CorrelationId!.Value);
                    if (temp is null) throw new InvalidOperationException("AssignmentTemp not found");

                    var assignmentTemp = _mapper.Map<IAssignmentTemp>(temp);

                    var assigment = assignmentFactory.ConvertFromTemp(assignmentTemp, ctx.Message.Id);

                    await assignmentService.CreateWithoutPublish(assigment);

                    await assignmentTempService.DeleteAssignmentTempAsync(assignmentTemp.Id);

                    await ctx.Publish(new AssignmentCreatedMessage(assigment.Id, assigment.DeviceId, assigment.CollaboratorId, assigment.PeriodDate.InitDate, assigment.PeriodDate.FinalDate));

                })
                .TransitionTo(Completed)
                .Finalize()
        );

        SetCompletedWhenFinalized();
    }

}