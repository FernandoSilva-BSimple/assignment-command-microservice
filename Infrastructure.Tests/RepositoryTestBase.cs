using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Infrastructure.Tests;

public class RepositoryTestBase
{
    protected readonly Mock<IMapper> _mapper;
    protected readonly AssignmentContext context;

    protected RepositoryTestBase()
    {
        _mapper = new Mock<IMapper>();

        // Configure in-memory database
        var options = new DbContextOptionsBuilder<AssignmentContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) // unique DB per test
            .Options;

        context = new AssignmentContext(options);
    }
}
