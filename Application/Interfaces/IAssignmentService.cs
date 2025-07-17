using Application.DTO.Assignment;
using Domain.Interfaces;
using Domain.Models;

namespace Application.Interfaces;

public interface IAssignmentService
{
    Task<Result<CreatedAssignmentDTO>> Create(CreateAssignmentDTO createAssignmentDTO);
    Task<Result<UpdatedAssignmentDTO>> Update(UpdateAssignmentDTO dto);
    Task<IAssignment?> AddConsumedAssignmentAsync(Guid id, Guid collaboratorId, Guid deviceId, PeriodDate periodDate);
    Task<IAssignment?> UpdateConsumedAssignmentAsync(Guid id, Guid collaboratorId, Guid deviceId, PeriodDate periodDate);
}