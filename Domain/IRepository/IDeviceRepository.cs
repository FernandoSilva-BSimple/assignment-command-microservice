namespace Domain.IRepository;

public interface IDeviceRepository
{
    Task<bool> Exists(Guid id);
}