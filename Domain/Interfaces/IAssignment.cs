using Domain.Models;

namespace Domain.Interfaces;

public interface IAssignment
{
    public Guid Id { get; set; }
    public Guid DeviceId { get; set; }
    public Guid CollaboratorId { get; set; }
    public PeriodDate PeriodDate { get; set; }
}