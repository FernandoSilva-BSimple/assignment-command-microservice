namespace Domain.Messages
{
    public record AssignmentCreatedMessage(Guid AssignmentId, Guid DeviceId, Guid CollaboratorId, DateOnly StartDate, DateOnly EndDate);
}