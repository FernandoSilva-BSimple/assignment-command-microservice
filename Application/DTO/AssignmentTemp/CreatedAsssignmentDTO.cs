using Domain.Interfaces;
using Domain.Models;

namespace Application.DTO.AssignmentTemp;

public record CreatedAssignmentTempDTO
{
    public Guid Id { get; init; }
    public Guid CollaboratorId { get; init; }
    public PeriodDate PeriodDate { get; init; }
    public string DeviceDescription { get; init; }
    public string DeviceBrand { get; init; }
    public string DeviceModel { get; init; }
    public string DeviceSerialNumber { get; init; }

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