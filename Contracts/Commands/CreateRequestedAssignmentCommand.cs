namespace Contracts.Commands;

public record CreateRequestedAssignmentCommand(Guid AssignmentTempId, Guid CollaboratorId, DateOnly StartDate, DateOnly EndDate, string DeviceDescription, string DeviceBrand, string DeviceModel, string DeviceSerialNumber);