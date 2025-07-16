namespace Application.DTO.Device;

public record CreateDeviceDTO
{
    public Guid Id { get; }
    public CreateDeviceDTO(Guid guid)
    {
        this.Id = guid;
    }
    public CreateDeviceDTO() { }
}