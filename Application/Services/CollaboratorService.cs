using Application.Interfaces;
using Domain.Factory;
using Domain.Factory.CollaboratorFactory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;

namespace Application.Services
{
    public class CollaboratorService : ICollaboratorService
    {
        private readonly ICollaboratorRepository _collaboratorRepository;
        private readonly ICollaboratorFactory _collaboratorFactory;

        public CollaboratorService(ICollaboratorRepository CollaboratorRepository, ICollaboratorFactory CollaboratorFactory)
        {
            _collaboratorRepository = CollaboratorRepository;
            _collaboratorFactory = CollaboratorFactory;
        }

        public async Task<ICollaborator> AddConsumedCollaboratorAsync(Guid collaboratorId, PeriodDateTime periodDate)
        {
            var newCollaborator = _collaboratorFactory.Create(collaboratorId, periodDate);

            return await _collaboratorRepository.AddCollaboratorAsync(newCollaborator);
        }
    }
}