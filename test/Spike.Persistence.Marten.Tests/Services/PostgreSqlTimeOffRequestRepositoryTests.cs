using FluentAssertions;
using Spike.Domain.Models;
using Spike.Persistence.Marten.Services;

namespace Spike.Persistence.Marten.Tests.Services
{
    [Collection("Marten collection")]
    public class PostgreSqlTimeOffRequestRepositoryTests
    {
        private readonly MartenFixture fixture;

        public PostgreSqlTimeOffRequestRepositoryTests(MartenFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task Sandbox()
        {
            var id = TimeOffRequestId.New();

            // create
            var timeOffRequest = new TimeOffRequest(id, DateTime.Now, DateTime.Now.AddDays(1));
            timeOffRequest.Cancel();

            var repository = new PostgreSqlTimeOffRequestRepository(fixture.CreateDocumentStore());
            await repository.Save(timeOffRequest, CancellationToken.None);

            // read from event stream
            timeOffRequest = await repository.Hydrate(id, null, CancellationToken.None);
            timeOffRequest.Should().NotBeNull();

            // read from projection
            var timeOffRequestInfo = await repository.Get(id, CancellationToken.None);
            timeOffRequestInfo.Should().NotBeNull();
        }
    }
}
