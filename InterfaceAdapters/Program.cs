using Application.DTO;
using Application.DTO.Assignment;
using Application.DTO.AssignmentTemp;
using Application.DTO.Collaborator;
using Application.DTO.Device;
using Application.Interfaces;
using Application.IPublishers;
using Application.Services;
using Domain.Factory;
using Domain.Factory.AssignmentFactory;
using Domain.Factory.AssignmentTempFactory;
using Domain.Factory.CollaboratorFactory;
using Domain.Factory.DeviceFactory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Infrastructure;
using Infrastructure.Repositories;
using Infrastructure.Resolvers;
using InterfaceAdapters;
using InterfaceAdapters.Consumers;
using InterfaceAdapters.Publisher;
using InterfaceAdapters.Saga;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<AssignmentContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
    );

//Services
builder.Services.AddTransient<IDeviceService, DeviceService>();
builder.Services.AddTransient<ICollaboratorService, CollaboratorService>();
builder.Services.AddTransient<IAssignmentService, AssignmentService>();
builder.Services.AddTransient<IAssignmentTempService, AssignmentTempService>();


// Publisher and fake publisher
if (builder.Environment.IsEnvironment("Test"))
{
    builder.Services.AddScoped<IMessagePublisher, FakePublisherForTests>();
}
else
{
    builder.Services.AddTransient<IMessagePublisher, MassTransitPublisher>();
}

//Repositories
builder.Services.AddTransient<IDeviceRepository, DeviceRepository>();
builder.Services.AddTransient<ICollaboratorRepository, CollaboratorRepository>();
builder.Services.AddTransient<IAssignmentRepository, AssignmentRepository>();
builder.Services.AddTransient<IAssignmentTempTempRepository, AssignmentTempRepository>();


//Factories
builder.Services.AddScoped<IDeviceFactory, DeviceFactory>();
builder.Services.AddScoped<ICollaboratorFactory, CollaboratorFactory>();
builder.Services.AddScoped<IAssignmentFactory, AssignmentFactory>();
builder.Services.AddScoped<IAssignmentTempFactory, AssignmentTempFactory>();


//Mappers
builder.Services.AddTransient<DeviceDataModelConverter>();
builder.Services.AddTransient<CollaboratorDataModelConverter>();
builder.Services.AddTransient<AssignmentDataModelConverter>();
builder.Services.AddTransient<AssignmentTempDataModelConverter>();

builder.Services.AddAutoMapper(cfg =>
{
    //DataModels
    cfg.AddProfile<DataModelMappingProfile>();

    //DTO
    cfg.CreateMap<Device, DeviceDTO>();
    cfg.CreateMap<DeviceDTO, Device>();

    cfg.CreateMap<Collaborator, CollaboratorDTO>();
    cfg.CreateMap<CollaboratorDTO, Collaborator>();

    cfg.CreateMap<Assignment, AssignmentDTO>();
    cfg.CreateMap<AssignmentDTO, Assignment>();

    cfg.CreateMap<AssignmentTemp, AssignmentTempDTO>();
    cfg.CreateMap<AssignmentTempDTO, AssignmentTemp>();

    cfg.CreateMap<AssignmentTempDTO, IAssignmentTemp>()
    .As<AssignmentTemp>();
});


// MassTransit
var instanceId = InstanceInfo.InstanceId;

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<DeviceCreatedConsumer>();
    x.AddConsumer<AssignmentCreatedConsumer>();
    x.AddConsumer<AssignmentUpdatedConsumer>();
    x.AddConsumer<CollaboratorCreatedConsumer>();

    x.AddSagaStateMachine<AssignmentSaga, AssignmentSagaState>()
            .InMemoryRepository();

    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("localhost", 5674, "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint($"assignments-cmd-{instanceId}", e =>
        {
            e.ConfigureConsumer<DeviceCreatedConsumer>(ctx);
        });

        cfg.ReceiveEndpoint($"assignments-cmd-saga-{instanceId}", e =>
        {
            e.StateMachineSaga<AssignmentSagaState>(ctx);
        });
    });
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();



app.UseCors(builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowed((host) => true)
                .AllowCredentials());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
