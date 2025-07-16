using Domain.Interfaces;
using Domain.Models;
using Domain.Visitors;

namespace Infrastructure.DataModel
{
    public class CollaboratorDataModel : ICollaboratorVisitor
    {
        public Guid Id { get; set; }
        public PeriodDateTime PeriodDateTime { get; set; }

        public CollaboratorDataModel() { }

        public CollaboratorDataModel(ICollaborator collaborator)
        {
            Id = collaborator.Id;
            PeriodDateTime = collaborator.PeriodDateTime;
        }
    }
}