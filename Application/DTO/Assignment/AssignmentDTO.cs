using Domain.Interfaces;
using Domain.Models;

namespace Application.DTO.Assignment;

public record AssignmentDTO
{
    public Guid Id { get; }
    public Guid DeviceId { get; }
    public Guid CollaboratorId { get; }
    public PeriodDate PeriodDate { get; }

    public AssignmentDTO(Guid id, Guid deviceId, Guid collaboratorId, PeriodDate periodDate)
    {
        this.Id = id;
        this.DeviceId = deviceId;
        this.CollaboratorId = collaboratorId;
        this.PeriodDate = periodDate;
    }

    public AssignmentDTO() { }
}