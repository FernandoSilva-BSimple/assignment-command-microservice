using Domain.Interfaces;

namespace Domain.IRepository;

public interface ICollaboratorRepository
{
    Task<ICollaborator?> GetByIdAsync(Guid id);
    Task<ICollaborator> AddCollaboratorAsync(ICollaborator collaborator);

}