using Domain.Interfaces;
using Domain.Models;
using Domain.Visitors;

namespace Domain.Factory.AssignmentTempFactory;

public interface IAssignmentTempFactory
{
    IAssignmentTemp Create(Guid id, Guid collaboratorId, PeriodDate periodDate, string deviceDescription, string deviceBrand, string deviceModel, string deviceSerialNumber);
    Task<IAssignmentTemp> Create(Guid collaboratorId, PeriodDate periodDate, string deviceDescription, string deviceBrand, string deviceModel, string deviceSerialNumber);
    IAssignmentTemp Create(IAssignmentTempVisitor visitor);
}