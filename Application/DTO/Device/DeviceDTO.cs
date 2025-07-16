namespace Application.DTO.Device;

public record DeviceDTO
{
    public Guid Id { get; }
    public DeviceDTO(Guid guid)
    {
        this.Id = guid;
    }
    public DeviceDTO() { }
}