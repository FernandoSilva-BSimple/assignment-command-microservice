using AutoMapper;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Infrastructure.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AssignmentRepository : IAssignmentRepository
{
    private readonly IMapper _mapper;
    private readonly AssignmentContext _context;

    public AssignmentRepository(IMapper mapper, AssignmentContext context)
    {
        _mapper = mapper;
        _context = context;
    }
    public async Task<IAssignment> CreateAssignmentAsync(IAssignment assignment)
    {
        var assignmentDM = _mapper.Map<AssignmentDataModel>(assignment);

        await _context.Set<AssignmentDataModel>().AddAsync(assignmentDM);
        await _context.SaveChangesAsync();

        var assignmentAdded = _mapper.Map<IAssignment>(assignmentDM);
        return assignmentAdded;
    }

    public async Task<bool> ExistsWithDeviceAndOverlappingPeriod(Guid deviceId, PeriodDate period)
    {
        var exists = await _context.Set<AssignmentDataModel>()
            .AnyAsync(a =>
                a.DeviceId == deviceId &&
                a.PeriodDate.InitDate <= period.FinalDate &&
                a.PeriodDate.FinalDate >= period.InitDate
            );

        return exists;
    }

    public async Task<bool> ExistsWithDeviceAndOverlappingPeriodExcept(Guid deviceId, PeriodDate period, Guid excludeAssignmentId)
    {
        return await _context.Set<AssignmentDataModel>()
            .AnyAsync(a =>
                a.DeviceId == deviceId &&
                a.Id != excludeAssignmentId &&
                a.PeriodDate.InitDate <= period.FinalDate &&
                a.PeriodDate.FinalDate >= period.InitDate
            );
    }


    public async Task<IAssignment?> GetAssignmentByIdAsync(Guid id)
    {
        var assignmentDM = await _context.Set<AssignmentDataModel>().FirstOrDefaultAsync(a => a.Id == id);

        if (assignmentDM == null) return null;

        var assignment = _mapper.Map<IAssignment>(assignmentDM);
        return assignment;
    }

    public async Task<IAssignment?> UpdateAssignmentAsync(IAssignment assignment)
    {
        var assignmentDM = await _context.Set<AssignmentDataModel>().FirstOrDefaultAsync(a => a.Id == assignment.Id);
        if (assignmentDM == null) return null;

        assignmentDM.DeviceId = assignment.DeviceId;
        assignmentDM.CollaboratorId = assignment.CollaboratorId;
        assignmentDM.PeriodDate = assignment.PeriodDate;

        _context.Set<AssignmentDataModel>().Update(assignmentDM);
        await _context.SaveChangesAsync();

        var assignmentUpdated = _mapper.Map<IAssignment>(assignmentDM);
        return assignmentUpdated;
    }
}