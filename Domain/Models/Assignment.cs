using Domain.Interfaces;

namespace Domain.Models;

public class Assignment : IAssignment
{
    public Guid Id { get; set; }
    public Guid DeviceId { get; set; }
    public Guid CollaboratorId { get; set; }
    public PeriodDate PeriodDate { get; set; }

    public Assignment(Guid id, Guid deviceId, Guid collaboratorId, PeriodDate periodDate)
    {
        Id = id;
        DeviceId = deviceId;
        CollaboratorId = collaboratorId;
        PeriodDate = periodDate;
    }

    public Assignment(Guid deviceId, Guid collaboratorId, PeriodDate periodDate)
    {
        Id = Guid.NewGuid();
        DeviceId = deviceId;
        CollaboratorId = collaboratorId;
        PeriodDate = periodDate;
    }

    public Assignment() { }
}