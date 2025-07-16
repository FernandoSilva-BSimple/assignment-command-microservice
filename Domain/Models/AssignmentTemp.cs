using Domain.Interfaces;

namespace Domain.Models;

public class AssignmentTemp : IAssignmentTemp
{
    public Guid Id { get; set; }
    public Guid CollaboratorId { get; set; }
    public PeriodDate PeriodDate { get; set; }
    public string DeviceDescription { get; set; }
    public string DeviceBrand { get; set; }
    public string DeviceModel { get; set; }
    public string DeviceSerialNumber { get; set; }

    public AssignmentTemp() { }

    public AssignmentTemp(Guid collaboratorId, PeriodDate periodDate, string deviceDescription, string deviceBrand, string deviceModel, string deviceSerialNumber)
    {
        Id = Guid.NewGuid();
        CollaboratorId = collaboratorId;
        PeriodDate = periodDate;
        DeviceDescription = deviceDescription;
        DeviceBrand = deviceBrand;
        DeviceModel = deviceModel;
        DeviceSerialNumber = deviceSerialNumber;
    }

    public AssignmentTemp(Guid id, Guid collaboratorId, PeriodDate periodDate, string deviceDescription, string deviceBrand, string deviceModel, string deviceSerialNumber)
    {
        Id = id;
        CollaboratorId = collaboratorId;
        PeriodDate = periodDate;
        DeviceDescription = deviceDescription;
        DeviceBrand = deviceBrand;
        DeviceModel = deviceModel;
        DeviceSerialNumber = deviceSerialNumber;
    }
}