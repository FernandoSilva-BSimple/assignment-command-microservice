using System.Threading.Tasks;
using Domain.IRepository;
using Domain.Models;
using Domain.Visitors;

namespace Domain.Factory.AssignmentFactory;

public class AssignmentFactory
{
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly ICollaboratorRepository _collaboratorRepository;
    private readonly IDeviceRepository _deviceRepository;

    public AssignmentFactory(IAssignmentRepository assignmentRepository, ICollaboratorRepository collaboratorRepository, IDeviceRepository deviceRepository)
    {
        _assignmentRepository = assignmentRepository;
        _collaboratorRepository = collaboratorRepository;
        _deviceRepository = deviceRepository;
    }

    public Assignment Create(Guid id, Guid deviceId, Guid collaboratorId, PeriodDate periodDate)
    {
        return new Assignment(id, deviceId, collaboratorId, periodDate);
    }

    public async Task<Assignment> Create(Guid deviceId, Guid collaboratorId, PeriodDate periodDate)
    {

        var collaborator = await _collaboratorRepository.GetByIdAsync(collaboratorId);
        if (collaborator == null) throw new Exception("Collaborator not found");

        var collaboratorStart = DateOnly.FromDateTime(collaborator.PeriodDateTime._initDate);
        var collaboratorEnd = DateOnly.FromDateTime(collaborator.PeriodDateTime._finalDate);

        if (periodDate.InitDate < collaboratorStart || periodDate.FinalDate > collaboratorEnd)
            throw new Exception("Assignment period must be within the collaborator's active period");

        var deviceExists = await _deviceRepository.Exists(deviceId);
        if (!deviceExists) throw new Exception("Device not found");

        var deviceHasOverlap = await _assignmentRepository.ExistsWithDeviceAndOverlappingPeriod(deviceId, periodDate);
        if (deviceHasOverlap) throw new Exception("Device already has an assigment overlapping with this period");

        return new Assignment(deviceId, collaboratorId, periodDate);
    }

    public Assignment Create(IAssignmentVisitor visitor)
    {
        return new Assignment(visitor.Id, visitor.DeviceId, visitor.CollaboratorId, visitor.PeriodDate);
    }
}


