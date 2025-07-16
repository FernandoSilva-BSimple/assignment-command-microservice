using Domain.Models;
using Domain.Visitors;

namespace Domain.Factory.CollaboratorFactory;

public class CollaboratorFactory : ICollaboratorFactory
{
    public CollaboratorFactory() { }

    public Collaborator Create(Guid id, PeriodDateTime periodDateTime)
    {
        return new Collaborator(id, periodDateTime);
    }

    public Collaborator Create(ICollaboratorVisitor visitor)
    {
        return new Collaborator(visitor.Id, visitor.PeriodDateTime);
    }
}