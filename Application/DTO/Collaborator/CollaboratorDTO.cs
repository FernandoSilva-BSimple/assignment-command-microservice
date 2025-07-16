using Domain.Models;

namespace Application.DTO.Collaborator;

public record CollaboratorDTO
{
    public Guid Id { get; }
    public PeriodDate PeriodDate { get; }
    public CollaboratorDTO(Guid guid, PeriodDate periodDate)
    {
        this.Id = guid;
        this.PeriodDate = periodDate;
    }
    public CollaboratorDTO() { }
}