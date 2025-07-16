using Domain.Interfaces;
using Domain.Models;
using Domain.Visitors;

namespace Domain.Factory.AssignmentTempFactory;

public interface IAssignmentFactory
{
    AssignmentTemp Create(Guid id, Guid collaboratorId, PeriodDate periodDate, string deviceDescription, string deviceBrand, string deviceModel, string deviceSerialNumber);
    AssignmentTemp Create(Guid collaboratorId, PeriodDate periodDate, string deviceDescription, string deviceBrand, string deviceModel, string deviceSerialNumber);
    AssignmentTemp Create(IAssignmentTempVisitor visitor);

}