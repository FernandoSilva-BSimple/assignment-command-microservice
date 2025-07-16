using Domain.Interfaces;
using Domain.Models;

namespace Domain.IRepository;

public interface IAssignmentRepository
{
    Task<IAssignment?> GetAssignmentByIdAsync(Guid id);
    Task<IAssignment> CreateAssignmentAsync(Assignment assignment);
    Task<IAssignment> UpdateAssignmentAsync(Assignment assignment);
    Task<bool> ExistsWithDeviceAndOverlappingPeriod(Guid deviceId, PeriodDate period);
}