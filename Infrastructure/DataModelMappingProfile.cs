using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Resolvers;

namespace Infrastructure;

public class DataModelMappingProfile : Profile
{
    public DataModelMappingProfile()
    {
        CreateMap<Device, DeviceDataModel>();
        CreateMap<DeviceDataModel, IDevice>()
            .ConvertUsing<DeviceDataModelConverter>();
        CreateMap<Collaborator, CollaboratorDataModel>();
        CreateMap<CollaboratorDataModel, ICollaborator>()
            .ConvertUsing<CollaboratorDataModelConverter>();
        CreateMap<AssignmentTemp, AssignmentTempDataModel>();
        CreateMap<AssignmentTempDataModel, IAssignmentTemp>()
            .ConvertUsing<AssignmentTempDataModelConverter>();
        CreateMap<Assignment, AssignmentDataModel>();
        CreateMap<AssignmentDataModel, IAssignment>()
            .ConvertUsing<AssignmentDataModelConverter>();
    }

}