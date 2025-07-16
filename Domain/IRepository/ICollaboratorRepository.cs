using Domain.Interfaces;

namespace Domain.IRepository;

public interface ICollaboratorRepository
{
    Task<bool> Exists(Guid id);
    Task<ICollaborator> GetById(Guid id);

}