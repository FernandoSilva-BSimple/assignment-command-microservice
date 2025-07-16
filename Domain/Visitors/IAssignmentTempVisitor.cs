using Domain.Models;

namespace Domain.Visitors;

public interface IAssignmentTempVisitor
{
    public Guid Id { get; }
    public Guid CollaboratorId { get; }
    public PeriodDate PeriodDate { get; }
    public string DeviceDescription { get; }
    public string DeviceBrand { get; }
    public string DeviceModel { get; }
    public string DeviceSerialNumber { get; }
}


