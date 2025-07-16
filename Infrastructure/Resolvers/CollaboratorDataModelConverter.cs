using AutoMapper;
using Domain.Factory.CollaboratorFactory;
using Domain.Interfaces;
using Infrastructure.DataModel;

namespace Infrastructure.Resolvers;

public class CollaboratorDataModelConverter : ITypeConverter<CollaboratorDataModel, ICollaborator>
{
    private readonly ICollaboratorFactory _collaboratorFactoryfactory;

    public CollaboratorDataModelConverter(ICollaboratorFactory factory)
    {
        _collaboratorFactoryfactory = factory;
    }

    public ICollaborator Convert(CollaboratorDataModel source, ICollaborator destination, ResolutionContext context)
    {
        return _collaboratorFactoryfactory.Create(source);
    }
}