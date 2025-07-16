using Domain.Models;

namespace Domain.Interfaces;

public interface IAssignment
{
    public Guid Id { get; }
    public Guid DeviceId { get; }
    public Guid CollaboratorId { get; }
    public PeriodDate PeriodDate { get; }
    public void UpdateDevice(Guid newDeviceId);

    public void UpdateCollaborator(Guid newCollaboratorId);

    public void UpdatePeriodDate(PeriodDate newPeriodDate);
}