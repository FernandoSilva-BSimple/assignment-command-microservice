using AutoMapper;
using Domain.Interfaces;
using Domain.IRepository;
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
}