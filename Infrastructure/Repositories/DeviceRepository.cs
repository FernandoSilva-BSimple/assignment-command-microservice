using AutoMapper;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class DeviceRepository : IDeviceRepository
{
    private readonly IMapper _mapper;
    private readonly AssignmentContext _context;

    public DeviceRepository(IMapper mapper, AssignmentContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<bool> Exists(Guid id)
    {
        var exists = await _context.Devices.AnyAsync(d => d.Id == id);
        return exists;
    }
}