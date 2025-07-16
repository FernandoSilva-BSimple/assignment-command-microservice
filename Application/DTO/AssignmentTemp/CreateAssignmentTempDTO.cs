using Domain.Interfaces;
using Domain.Models;

namespace Application.DTO.AssignmentTemp;

public record CreateAssignmentTempDTO
{
    public Guid CollaboratorId { get; }
    public PeriodDate PeriodDate { get; }
    public string DeviceDescription { get; }
    public string DeviceBrand { get; }
    public string DeviceModel { get; }
    public string DeviceSerialNumber { get; }

    public CreateAssignmentTempDTO(Guid collaboratorId, PeriodDate periodDate, string deviceDescription, string deviceBrand, string deviceModel, string deviceSerialNumber)
    {
        this.CollaboratorId = collaboratorId;
        this.PeriodDate = periodDate;
        this.DeviceDescription = deviceDescription;
        this.DeviceBrand = deviceBrand;
        this.DeviceModel = deviceModel;
        this.DeviceSerialNumber = deviceSerialNumber;
    }

    public CreateAssignmentTempDTO() { }
}