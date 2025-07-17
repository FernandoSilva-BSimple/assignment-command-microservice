namespace Domain.Messages
{
    public record AssignmentUpdatedMessage(Guid AssignmentId, Guid DeviceId, Guid CollaboratorId, DateOnly StartDate, DateOnly EndDate);
}