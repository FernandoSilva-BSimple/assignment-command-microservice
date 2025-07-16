using Domain.Models;

namespace Application.DTO.Assignment;

public record CreatedAssignmentDTO
{
    public Guid Id { get; init; }
    public Guid DeviceId { get; init; }
    public Guid CollaboratorId { get; init; }
    public PeriodDate PeriodDate { get; init; }

    public CreatedAssignmentDTO(Guid id, Guid deviceId, Guid collaboratorId, PeriodDate periodDate)
    {
        this.Id = id;
        this.DeviceId = deviceId;
        this.CollaboratorId = collaboratorId;
        this.PeriodDate = periodDate;
    }

    public CreatedAssignmentDTO() { }
}