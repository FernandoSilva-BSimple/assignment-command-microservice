using Application.DTO.Assignment;
using Application.DTO.AssignmentTemp;
using Application.IPublishers;
using Contracts.Commands;
using Domain.Factory.AssignmentTempFactory;
using Domain.IRepository;

namespace Application.Services
{
    public class AssignmentTempService
    {
        private readonly IAssignmentTempTempRepository _assignmentTempRepository;
        private readonly IAssignmentTempFactory _assignmentTempFactory;
        private readonly IMessagePublisher _messagePublisher;

        public AssignmentTempService(IAssignmentTempTempRepository assignmentTempRepository, IAssignmentTempFactory assignmentTempFactory, IMessagePublisher messagePublisher)
        {
            _assignmentTempRepository = assignmentTempRepository;
            _assignmentTempFactory = assignmentTempFactory;
            _messagePublisher = messagePublisher;
        }

        public async Task CreateAssignmentTempAsync(CreateAssignmentTempDTO createAssignmentTempDTO)
        {
            var assignmentTemp = await _assignmentTempFactory.Create(createAssignmentTempDTO.CollaboratorId, createAssignmentTempDTO.PeriodDate, createAssignmentTempDTO.DeviceDescription, createAssignmentTempDTO.DeviceBrand, createAssignmentTempDTO.DeviceModel, createAssignmentTempDTO.DeviceSerialNumber);
            await _assignmentTempRepository.CreateAssignmentTempAsync(assignmentTemp);
        }

        public async Task StartSagaAsync(CreateAssignmentAndDeviceDTO dto)
        {
            CreateRequestedAssignmentCommand command = new(dto.CollaboratorId, dto.PeriodDate.InitDate, dto.PeriodDate.FinalDate, dto.DeviceDescription, dto.DeviceBrand, dto.DeviceModel, dto.DeviceSerialNumber);
            await _messagePublisher.SendCreateAssignmentSagaCommandAsync(command);
        }

        public async Task DeleteAssignmentTempAsync(Guid id)
        {
            var existing = await _assignmentTempRepository.GetAssignmentTempByIdAsync(id);
            if (existing == null) throw new InvalidOperationException("AssignmentTemp not found");

            await _assignmentTempRepository.RemoveAssignmentTempAsync(existing);
        }

        public async Task<AssignmentTempDTO?> GetByIdAsync(Guid id)
        {
            var assignmentTemp = await _assignmentTempRepository.GetAssignmentTempByIdAsync(id);
            if (assignmentTemp == null)
                return null;

            return new AssignmentTempDTO(
                assignmentTemp.Id,
                assignmentTemp.CollaboratorId,
                assignmentTemp.PeriodDate,
                assignmentTemp.DeviceDescription,
                assignmentTemp.DeviceBrand,
                assignmentTemp.DeviceModel,
                assignmentTemp.DeviceSerialNumber
            );
        }


    }
}