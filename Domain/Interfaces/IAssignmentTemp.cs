using Domain.Models;

namespace Domain.Interfaces;

public interface IAssignmentTemp
{
    public Guid Id { get; set; }
    public Guid CollaboratorId { get; set; }
    public PeriodDate PeriodDate { get; set; }
    public string DeviceDescription { get; set; }
    public string DeviceBrand { get; set; }
    public string DeviceModel { get; set; }
    public string DeviceSerialNumber { get; set; }
}