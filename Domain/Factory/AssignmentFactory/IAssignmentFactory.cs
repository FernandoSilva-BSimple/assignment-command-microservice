using Domain.Interfaces;
using Domain.Models;
using Domain.Visitors;

namespace Domain.Factory.AssignmentFactory;

public interface IAssignmentFactory
{
    IAssignment Create(Guid id, Guid deviceId, Guid collaboratorId, PeriodDate periodDate);
    Task<IAssignment> Create(Guid deviceId, Guid collaboratorId, PeriodDate periodDate);
    IAssignment Create(IAssignmentVisitor visitor);
    IAssignment ConvertFromTemp(IAssignmentTemp assignmentTemp, Guid deviceId);

}