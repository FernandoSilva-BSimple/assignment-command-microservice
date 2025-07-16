using AutoMapper;
using Domain.Interfaces;
using Domain.IRepository;
using Infrastructure.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CollaboratorRepository : ICollaboratorRepository
{
    private readonly IMapper _mapper;
    private readonly AssignmentContext _context;

    public CollaboratorRepository(IMapper mapper, AssignmentContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<ICollaborator?> GetByIdAsync(Guid id)
    {
        var collaboratorDM = await _context.Collaborators.FirstOrDefaultAsync(c => c.Id == id);
        var collab = _mapper.Map<ICollaborator>(collaboratorDM);
        return collab;
    }

    public async Task<ICollaborator> AddCollaboratorAsync(ICollaborator collaborator)
    {
        var collaboratorDM = _mapper.Map<CollaboratorDataModel>(collaborator);

        await _context.Collaborators.AddAsync(collaboratorDM);
        await _context.SaveChangesAsync();

        var collaboratorAdded = _mapper.Map<ICollaborator>(collaboratorDM);
        return collaboratorAdded;
    }
}