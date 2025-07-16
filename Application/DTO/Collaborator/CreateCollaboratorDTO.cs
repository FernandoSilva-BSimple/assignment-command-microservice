using Domain.Models;

namespace Application.DTO.Collaborator;

public record CreateCollaboratorDTO
{
    public Guid Id { get; }
    public PeriodDate PeriodDate { get; }
    public CreateCollaboratorDTO(Guid guid, PeriodDate periodDate)
    {
        this.Id = guid;
        this.PeriodDate = periodDate;
    }
    public CreateCollaboratorDTO() { }
}