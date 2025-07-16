using AutoMapper;
using Domain.Factory;
using Domain.Factory.AssignmentTempFactory;
using Domain.Interfaces;
using Domain.Models;
using Infrastructure.DataModel;

namespace Infrastructure.Resolvers;

public class AssignmentTempDataModelConverter : ITypeConverter<AssignmentTempDataModel, IAssignmentTemp>
{
    private readonly IAssignmentTempFactory _factory;

    public AssignmentTempDataModelConverter(IAssignmentTempFactory factory)
    {
        _factory = factory;
    }

    public IAssignmentTemp Convert(AssignmentTempDataModel source, IAssignmentTemp destination, ResolutionContext context)
    {
        return _factory.Create(source);
    }
}