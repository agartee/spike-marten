using Moq;
using Spike.Domain.Commands;
using Spike.Domain.Services;

namespace Spike.Domain.Tests.Commands
{
    public class CreateTimeOffRequestTests
    {
        [Fact]
        public async Task Handle_CreatesEventStreamAndProjections()
        {
            var repository = new Mock<ITimeOffRequestRepository>();
            var handler = new CreateTimeOffRequestHandler(repository.Object);

            var result = await handler.Handle(new CreateTimeOffRequest
            {
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1)
            }, CancellationToken.None);
        }
    }
}
