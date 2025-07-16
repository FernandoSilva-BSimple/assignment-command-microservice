using AutoMapper;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Infrastructure.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AssignmentTempRepository : IAssignmentTempTempRepository
{
    private readonly IMapper _mapper;
    private readonly AssignmentContext _context;

    public AssignmentTempRepository(IMapper mapper, AssignmentContext context)
    {
        _mapper = mapper;
        _context = context;
    }
    public async Task<IAssignmentTemp> CreateAssignmentTempAsync(IAssignmentTemp assignmentTemp)
    {
        var assignmentTempDM = _mapper.Map<AssignmentTempDataModel>(assignmentTemp);

        await _context.Set<AssignmentTempDataModel>().AddAsync(assignmentTempDM);
        await _context.SaveChangesAsync();

        var assignmentTempAdded = _mapper.Map<IAssignmentTemp>(assignmentTempDM);
        return assignmentTempAdded;
    }

    public async Task<IAssignmentTemp?> GetAssignmentTempByIdAsync(Guid id)
    {
        var assignmentTempDM = await _context.Set<AssignmentTempDataModel>().AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        if (assignmentTempDM == null) return null;

        var assignmentTemp = _mapper.Map<IAssignmentTemp>(assignmentTempDM);
        return assignmentTemp;
    }

    public async Task RemoveAssignmentTempAsync(IAssignmentTemp assignmentTemp)
    {
        var assignmentTempDM = _mapper.Map<AssignmentTempDataModel>(assignmentTemp);
        _context.Set<AssignmentTempDataModel>().Remove(assignmentTempDM);
        await _context.SaveChangesAsync();
    }
}