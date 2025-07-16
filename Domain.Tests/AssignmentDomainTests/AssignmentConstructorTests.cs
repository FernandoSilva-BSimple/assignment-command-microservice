using Domain.Models;
using Moq;

namespace Domain.Tests.AssignmentDomainTests;

public class AssignmentConstructorTests
{
    [Fact]
    public void WhenCreatingAssignmentWithId_ThenAssignmentIsCreated()
    {
        //arrange

        //act & assert
        new Assignment(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<PeriodDate>());
    }

    [Fact]
    public void WhenCreatingAssignment_ThenAssignmentIsCreated()
    {
        //arrange

        //act & assert
        new Assignment(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<PeriodDate>());
    }
}