using Domain.Models;

namespace Application.DTO.Assignment;

public record CreateAssignmentDTO
{
    public Guid DeviceId { get; }
    public Guid CollaboratorId { get; }
    public PeriodDate PeriodDate { get; }

    public CreateAssignmentDTO(Guid deviceId, Guid collaboratorId, PeriodDate periodDate)
    {
        this.DeviceId = deviceId;
        this.CollaboratorId = collaboratorId;
        this.PeriodDate = periodDate;
    }

    public CreateAssignmentDTO() { }
}