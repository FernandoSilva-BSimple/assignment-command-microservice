using Domain.Interfaces;

namespace Domain.Models;

public class Device : IDevice
{
    public Guid Id { get; set; }

    public Device() { }

    public Device(Guid id)
    {
        Id = id;
    }
}