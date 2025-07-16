using Domain.Models;

namespace Application.DTO.Assignment;

public record CreateAssignmentDTO
{
    public Guid DeviceId { get; init; }
    public Guid CollaboratorId { get; init; }
    public PeriodDate PeriodDate { get; init; }

    public CreateAssignmentDTO(Guid deviceId, Guid collaboratorId, PeriodDate periodDate)
    {
        this.DeviceId = deviceId;
        this.CollaboratorId = collaboratorId;
        this.PeriodDate = periodDate;
    }

    public CreateAssignmentDTO() { }
}