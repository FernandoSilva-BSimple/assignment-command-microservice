using System.Net;
using Application.DTO.Assignment;
using Domain.Models;
using Newtonsoft.Json;
using WebApi.IntegrationTests;
using WebApi.IntegrationTests.Tests;
using Xunit;

namespace InterfaceAdapters.Tests.ControllerTests;

public class AssignmentControllerTests : IntegrationTestBase, IClassFixture<IntegrationTestsWebApplicationFactory<Program>>
{
    public AssignmentControllerTests(IntegrationTestsWebApplicationFactory<Program> factory)
        : base(factory.CreateClient()) { }

    [Fact]
    public async Task CreateAssignmentWithDevice_Returns202Accepted()
    {
        // Arrange
        var period = new PeriodDate(new DateOnly(2025, 9, 1), new DateOnly(2025, 9, 10));
        var dto = new CreateAssignmentAndDeviceDTO(
            Guid.NewGuid(),
            period,
            "Laptop de testes",
            "Dell",
            "Latitude 15",
            "SN123456"
        );

        // Act
        var response = await PostAsync("/api/assignments/with-device", dto);

        // Assert
        Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
    }
}
