using Domain.Interfaces;

namespace Domain.Models;

public class Assignment : IAssignment
{
    public Guid Id { get; private set; }
    public Guid DeviceId { get; private set; }
    public Guid CollaboratorId { get; private set; }
    public PeriodDate PeriodDate { get; private set; }

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

    public void UpdateDevice(Guid newDeviceId)
    {
        if (newDeviceId == Guid.Empty)
            throw new ArgumentException("Device ID cannot be empty");

        DeviceId = newDeviceId;
    }

    public void UpdateCollaborator(Guid newCollaboratorId)
    {
        if (newCollaboratorId == Guid.Empty)
            throw new ArgumentException("Collaborator ID cannot be empty");

        CollaboratorId = newCollaboratorId;
    }

    public void UpdatePeriodDate(PeriodDate newPeriodDate)
    {
        if (newPeriodDate is null)
            throw new ArgumentNullException(nameof(newPeriodDate));

        PeriodDate = newPeriodDate;
    }
}