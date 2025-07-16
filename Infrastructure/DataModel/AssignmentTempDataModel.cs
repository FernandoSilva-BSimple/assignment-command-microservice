using Domain.Interfaces;
using Domain.Models;
using Domain.Visitors;

namespace Infrastructure.DataModel;

public class AssignmentTempDataModel : IAssignmentTempVisitor
{
    public Guid Id { get; set; }

    public Guid CollaboratorId { get; set; }

    public PeriodDate PeriodDate { get; set; }

    public string DeviceDescription { get; set; }

    public string DeviceBrand { get; set; }

    public string DeviceModel { get; set; }

    public string DeviceSerialNumber { get; set; }

    public AssignmentTempDataModel(IAssignmentTemp assignmentTemp)
    {
        Id = assignmentTemp.Id;
        CollaboratorId = assignmentTemp.CollaboratorId;
        PeriodDate = assignmentTemp.PeriodDate;
        DeviceDescription = assignmentTemp.DeviceDescription;
        DeviceBrand = assignmentTemp.DeviceBrand;
        DeviceModel = assignmentTemp.DeviceModel;
        DeviceSerialNumber = assignmentTemp.DeviceSerialNumber;
    }

    public AssignmentTempDataModel() { }
}
