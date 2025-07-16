namespace Application.DTO.Device;

public record CreatedDeviceDTO
{
    public Guid Id { get; }
    public CreatedDeviceDTO(Guid guid)
    {
        this.Id = guid;
    }
    public CreatedDeviceDTO() { }
}