using Domain.Interfaces;
using Domain.Models;
using Domain.Visitors;

namespace Domain.Factory.AssignmentFactory;

public interface IAssignmentFactory
{
    IAssignment Create(Guid id, Guid deviceId, Guid collaboratorId, PeriodDate periodDate);
    Assignment Create(Guid deviceId, Guid collaboratorId, PeriodDate periodDate);
    Assignment Create(IAssignmentVisitor visitor);

}