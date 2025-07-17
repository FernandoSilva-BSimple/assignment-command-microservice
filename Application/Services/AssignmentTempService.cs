using Application.DTO.Assignment;
using Application.DTO.AssignmentTemp;
using Application.Interfaces;
using Application.IPublishers;
using Domain.Commands;
using Domain.Factory.AssignmentTempFactory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;

namespace Application.Services
{
    public class AssignmentTempService : IAssignmentTempService
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

        public async Task CreateAssignmentTempAsync(CreateRequestedAssignmentCommand command)
        {
            var period = new PeriodDate(command.StartDate, command.EndDate);

            var assignmentTemp = await _assignmentTempFactory.Create(command.AssignmentTempId, command.CollaboratorId, period, command.DeviceDescription, command.DeviceBrand, command.DeviceModel, command.DeviceSerialNumber);
            await _assignmentTempRepository.CreateAssignmentTempAsync(assignmentTemp);
        }

        public async Task StartSagaAsync(CreateAssignmentAndDeviceDTO dto)
        {
            Guid assignmentTempId = Guid.NewGuid();
            CreateRequestedAssignmentCommand command = new(assignmentTempId, dto.CollaboratorId, dto.PeriodDate.InitDate, dto.PeriodDate.FinalDate, dto.DeviceDescription, dto.DeviceBrand, dto.DeviceModel, dto.DeviceSerialNumber);
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