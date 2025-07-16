using Domain.Models;
using Moq;

namespace Domain.Tests.AssignmentTempDomainTests;

public class AssignmentTempConstructorTests
{
    [Fact]
    public void WhenCreatingAssignmentTempWithId_ThenAssignmentTempIsCreated()
    {
        //arrange

        //act & assert
        new AssignmentTemp(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<PeriodDate>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());
    }

    [Fact]
    public void WhenCreatingAssignmentTemp_ThenAssignmentTempIsCreated()
    {
        //arrange

        //act & assert
        new AssignmentTemp(It.IsAny<Guid>(), It.IsAny<PeriodDate>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());
    }
}