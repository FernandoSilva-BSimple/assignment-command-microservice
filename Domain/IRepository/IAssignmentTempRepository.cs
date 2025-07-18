using Domain.Interfaces;
using Domain.Models;

namespace Domain.IRepository;

public interface IAssignmentTempTempRepository
{
    Task<IAssignmentTemp?> GetAssignmentTempByIdAsync(Guid id);
    Task<IAssignmentTemp> CreateAssignmentTempAsync(IAssignmentTemp assignmentTemp);
    Task RemoveAssignmentTempAsync(IAssignmentTemp assignmentTemp);
}