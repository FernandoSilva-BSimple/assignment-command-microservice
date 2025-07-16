using Application.DTO.Assignment;
using Application.DTO.AssignmentTemp;
using Domain.Interfaces;
using Domain.Models;
using Contracts.Commands;

namespace Application.Interfaces;

public interface IAssignmentTempService
{
    Task CreateAssignmentTempAsync(CreateRequestedAssignmentCommand command);
    Task<IAssignmentTemp> CreateAssignmentTempAsyncWithId(Guid id, Guid CollabId, PeriodDate periodDate, string deviceDescription, string deviceBrand, string deviceModel, string deviceSerialNumber);
    Task StartSagaAsync(CreateAssignmentAndDeviceDTO dto);
    Task DeleteAssignmentTempAsync(Guid id);
    Task<AssignmentTempDTO?> GetByIdAsync(Guid id);
}