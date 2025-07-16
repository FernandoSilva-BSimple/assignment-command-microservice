using Domain.Models;

namespace Application.DTO.Collaborator;

public record CreatedCollaboratorDTO
{
    public Guid Id { get; }
    public PeriodDate PeriodDate { get; }
    public CreatedCollaboratorDTO(Guid guid, PeriodDate periodDate)
    {
        this.Id = guid;
        this.PeriodDate = periodDate;
    }
    public CreatedCollaboratorDTO() { }
}