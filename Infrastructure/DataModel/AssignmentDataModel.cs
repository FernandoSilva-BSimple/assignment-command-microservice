using Domain.Interfaces;
using Domain.Models;
using Domain.Visitors;

namespace Infrastructure.DataModel;

public class AssignmentDataModel : IAssignmentVisitor
{
    public Guid Id { get; set; }

    public Guid DeviceId { get; set; }

    public Guid CollaboratorId { get; set; }

    public PeriodDate PeriodDate { get; set; }

    public AssignmentDataModel() { }

    public AssignmentDataModel(IAssignment assignment)
    {
        Id = assignment.Id;
        DeviceId = assignment.DeviceId;
        CollaboratorId = assignment.CollaboratorId;
        PeriodDate = assignment.PeriodDate;
    }
}
