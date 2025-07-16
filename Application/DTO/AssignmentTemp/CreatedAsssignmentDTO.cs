using Domain.Interfaces;
using Domain.Models;

namespace Application.DTO.AssignmentTemp;

public record CreatedAssignmentTempDTO
{
    public Guid Id { get; }
    public Guid CollaboratorId { get; }
    public PeriodDate PeriodDate { get; }
    public string DeviceDescription { get; }
    public string DeviceBrand { get; }
    public string DeviceModel { get; }
    public string DeviceSerialNumber { get; }

    public CreatedAssignmentTempDTO(Guid id, Guid collaboratorId, PeriodDate periodDate, string deviceDescription, string deviceBrand, string deviceModel, string deviceSerialNumber)
    {
        this.Id = id;
        this.CollaboratorId = collaboratorId;
        this.PeriodDate = periodDate;
        this.DeviceDescription = deviceDescription;
        this.DeviceBrand = deviceBrand;
        this.DeviceModel = deviceModel;
        this.DeviceSerialNumber = deviceSerialNumber;
    }

    public CreatedAssignmentTempDTO() { }
}