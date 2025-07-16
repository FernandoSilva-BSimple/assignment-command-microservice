using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Domain.Visitors;

namespace Domain.Factory.AssignmentTempFactory;

public class AssignmentTempFactory : IAssignmentTempFactory
{
    private readonly ICollaboratorRepository _collaboratorRepository;

    public AssignmentTempFactory(ICollaboratorRepository collaboratorRepository)
    {
        _collaboratorRepository = collaboratorRepository;
    }

    public AssignmentTempFactory() { }

    public IAssignmentTemp Create(Guid id, Guid collaboratorId, PeriodDate periodDate, string deviceDescription, string deviceBrand, string deviceModel, string deviceSerialNumber)
    {
        return new AssignmentTemp(id, collaboratorId, periodDate, deviceDescription, deviceBrand, deviceModel, deviceSerialNumber);
    }

    public async Task<IAssignmentTemp> Create(Guid collaboratorId, PeriodDate periodDate, string deviceDescription, string deviceBrand, string deviceModel, string deviceSerialNumber)
    {

        var collaborator = await _collaboratorRepository.GetByIdAsync(collaboratorId);
        if (collaborator == null) throw new Exception("Collaborator not found");

        var collaboratorStart = DateOnly.FromDateTime(collaborator.PeriodDateTime._initDate);
        var collaboratorEnd = DateOnly.FromDateTime(collaborator.PeriodDateTime._finalDate);

        if (periodDate.InitDate < collaboratorStart || periodDate.FinalDate > collaboratorEnd)
            throw new Exception("Assignment period must be within the collaborator's active period");

        return new AssignmentTemp(collaboratorId, periodDate, deviceDescription, deviceBrand, deviceModel, deviceSerialNumber);
    }

    public IAssignmentTemp Create(IAssignmentTempVisitor visitor)
    {
        return new AssignmentTemp(visitor.Id, visitor.CollaboratorId, visitor.PeriodDate, visitor.DeviceDescription, visitor.DeviceBrand, visitor.DeviceModel, visitor.DeviceSerialNumber);
    }
}


