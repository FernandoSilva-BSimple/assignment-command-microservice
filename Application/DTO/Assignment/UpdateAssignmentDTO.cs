using Domain.Models;

namespace Application.DTO.Assignment;

public record UpdateAssignmentDTO
{
    public Guid Id { get; init; }
    public Guid DeviceId { get; init; }
    public Guid CollaboratorId { get; init; }
    public PeriodDate PeriodDate { get; init; }

    public UpdateAssignmentDTO(Guid id, Guid deviceId, Guid collaboratorId, PeriodDate periodDate)
    {
        this.Id = id;
        this.DeviceId = deviceId;
        this.CollaboratorId = collaboratorId;
        this.PeriodDate = periodDate;
    }

    public UpdateAssignmentDTO() { }
}