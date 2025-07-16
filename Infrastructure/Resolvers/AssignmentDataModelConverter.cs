using AutoMapper;
using Domain.Factory;
using Domain.Factory.AssignmentFactory;
using Domain.Interfaces;
using Domain.Models;
using Infrastructure.DataModel;

namespace Infrastructure.Resolvers;

public class AssignmentDataModelConverter : ITypeConverter<AssignmentDataModel, IAssignment>
{
    private readonly IAssignmentFactory _factory;

    public AssignmentDataModelConverter(IAssignmentFactory factory)
    {
        _factory = factory;
    }

    public IAssignment Convert(AssignmentDataModel source, IAssignment destination, ResolutionContext context)
    {
        return _factory.Create(source);
    }
}