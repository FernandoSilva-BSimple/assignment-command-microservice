
namespace Contracts.Messages
{
    public record CollaboratorCreatedMessage(Guid Id, Guid UserId, DateTime StartDate, DateTime EndDate);
}