using Domain.Interfaces;
using Domain.Models;

namespace Domain.IRepository;

public interface IAssignmentRepository
{
    Task<IAssignment?> GetAssignmentByIdAsync(Guid id);
    Task<IAssignment> CreateAssignmentAsync(IAssignment assignment);
    Task<IAssignment?> UpdateAssignmentAsync(IAssignment assignment);
    Task<bool> ExistsWithDeviceAndOverlappingPeriod(Guid deviceId, PeriodDate period);
    Task<bool> ExistsWithDeviceAndOverlappingPeriodExcept(Guid deviceId, PeriodDate period, Guid excludeAssignmentId);

}