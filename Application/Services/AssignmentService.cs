using Application.DTO.Assignment;
using Application.IPublishers;
using Domain.Factory.AssignmentFactory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;

namespace Application.Services
{
    public class AssignmentService
    {
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IAssignmentFactory _assignmentFactory;
        private readonly IDeviceRepository _deviceRepository;
        private readonly ICollaboratorRepository _collaboratorRepository;
        private readonly IMessagePublisher _publisher;

        public AssignmentService(IAssignmentRepository assignmentRepository, IAssignmentFactory assignmentFactory, IMessagePublisher publisher, IDeviceRepository deviceRepository, ICollaboratorRepository collaboratorRepository)
        {
            _assignmentRepository = assignmentRepository;
            _assignmentFactory = assignmentFactory;
            _publisher = publisher;
            _deviceRepository = deviceRepository;
            _collaboratorRepository = collaboratorRepository;
        }

        public async Task<Result<CreatedAssignmentDTO>> Create(CreateAssignmentDTO createAssignmentDTO)
        {
            IAssignment newAssignment;
            try
            {
                newAssignment = await _assignmentFactory.Create(createAssignmentDTO.DeviceId, createAssignmentDTO.CollaboratorId, createAssignmentDTO.PeriodDate);
                newAssignment = await _assignmentRepository.CreateAssignmentAsync(newAssignment);

                var result = new CreatedAssignmentDTO(newAssignment.Id, newAssignment.DeviceId, newAssignment.CollaboratorId, newAssignment.PeriodDate);

                await _publisher.PublishAssignmentCreatedAsync(newAssignment.Id, newAssignment.DeviceId, newAssignment.CollaboratorId, newAssignment.PeriodDate);

                return Result<CreatedAssignmentDTO>.Success(result);
            }
            catch (Exception ex)
            {
                return Result<CreatedAssignmentDTO>.Failure(Error.InternalServerError(ex.Message));

            }
        }

        public async Task<Result<UpdatedAssignmentDTO>> Update(UpdateAssignmentDTO dto)
        {
            try
            {
                var assignment = await _assignmentRepository.GetAssignmentByIdAsync(dto.Id);
                if (assignment == null)
                    return Result<UpdatedAssignmentDTO>.Failure(Error.NotFound("Assignment not found"));

                var collaborator = await _collaboratorRepository.GetByIdAsync(dto.CollaboratorId);
                if (collaborator == null)
                    return Result<UpdatedAssignmentDTO>.Failure(Error.NotFound("Collaborator not found"));

                var deviceExists = await _deviceRepository.Exists(dto.DeviceId);
                if (!deviceExists)
                    return Result<UpdatedAssignmentDTO>.Failure(Error.NotFound("Device not found"));

                var collabStart = DateOnly.FromDateTime(collaborator.PeriodDateTime._initDate);
                var collabEnd = DateOnly.FromDateTime(collaborator.PeriodDateTime._finalDate);

                if (dto.PeriodDate.InitDate < collabStart || dto.PeriodDate.FinalDate > collabEnd)
                    return Result<UpdatedAssignmentDTO>.Failure(Error.BadRequest("Assignment period must be within the collaborator's active period"));

                var hasConflict = await _assignmentRepository.ExistsWithDeviceAndOverlappingPeriodExcept(dto.DeviceId, dto.PeriodDate, dto.Id);
                if (hasConflict)
                    return Result<UpdatedAssignmentDTO>.Failure(Error.BadRequest("Device already has an assignment in this period"));

                assignment.UpdateDevice(dto.DeviceId);
                assignment.UpdateCollaborator(dto.CollaboratorId);
                assignment.UpdatePeriodDate(dto.PeriodDate);

                var updated = await _assignmentRepository.UpdateAssignmentAsync(assignment);
                if (updated == null)
                    return Result<UpdatedAssignmentDTO>.Failure(Error.InternalServerError("Update failed"));

                var result = new UpdatedAssignmentDTO(updated.Id, updated.DeviceId, updated.CollaboratorId, updated.PeriodDate);

                await _publisher.PublishAssignmentUpdatedAsync(updated.Id, updated.DeviceId, updated.CollaboratorId, updated.PeriodDate);

                return Result<UpdatedAssignmentDTO>.Success(result);
            }
            catch (Exception ex)
            {
                return Result<UpdatedAssignmentDTO>.Failure(Error.InternalServerError(ex.Message));
            }
        }

        public async Task<IAssignment> AddConsumedAssignmentAsync(Guid id, Guid collaboratorId, Guid deviceId, PeriodDate periodDate)
        {
            var assignmentAlreadyExists = await _assignmentRepository.GetAssignmentByIdAsync(id);
            if (assignmentAlreadyExists != null) return assignmentAlreadyExists;

            var assignment = _assignmentFactory.Create(id, collaboratorId, deviceId, periodDate);
            return await _assignmentRepository.CreateAssignmentAsync(assignment);
        }

        public async Task<IAssignment?> UpdateConsumedAssignmentAsync(Guid id, Guid collaboratorId, Guid deviceId, PeriodDate periodDate)
        {
            var existingAssignment = await _assignmentRepository.GetAssignmentByIdAsync(id);
            if (existingAssignment == null) return null;

            existingAssignment.UpdateCollaborator(collaboratorId);
            existingAssignment.UpdateDevice(deviceId);
            existingAssignment.UpdatePeriodDate(periodDate);

            return await _assignmentRepository.UpdateAssignmentAsync(existingAssignment);
        }

    }
}