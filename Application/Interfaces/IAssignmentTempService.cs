using Application.DTO.Assignment;
using Application.DTO.AssignmentTemp;
using Domain.Interfaces;
using Domain.Models;
using Domain.Commands;

namespace Application.Interfaces;

public interface IAssignmentTempService
{
    Task CreateAssignmentTempAsync(CreateRequestedAssignmentCommand command);
    Task StartSagaAsync(CreateAssignmentAndDeviceDTO dto);
    Task DeleteAssignmentTempAsync(Guid id);
    Task<AssignmentTempDTO?> GetByIdAsync(Guid id);
}