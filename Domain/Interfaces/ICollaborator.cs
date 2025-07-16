using Domain.Models;

namespace Domain.Interfaces;

public interface ICollaborator
{
    public Guid Id { get; }
    public PeriodDateTime PeriodDateTime { get; }

}