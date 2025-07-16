using Domain.Interfaces;
using Domain.Models;

namespace Application.Interfaces;

public interface ICollaboratorService
{
    Task<ICollaborator> AddConsumedCollaboratorAsync(Guid collaboratorId, PeriodDateTime periodDate);
}