using FluentAssertions;
using Marten;
using Marten.Events;
using Marten.Events.Projections;
using Microsoft.Extensions.DependencyInjection;
using Spike.Domain.Models;
using Spike.Persistence.Marten.Projections;
using Spike.Persistence.Marten.Services;
using Weasel.Core;

namespace Spike.Persistence.Marten.Tests.Services
{
    public class PostgreSqlTimeOffRequestRepositoryTests
    {
        private readonly Guid spikeId = new Guid("0195b9a5-8580-7c7b-bdf9-717eb0f4cb8c");

        private readonly PostgreSqlTimeOffRequestRepository repository;

        public PostgreSqlTimeOffRequestRepositoryTests()
        {
            var services = new ServiceCollection();

            services.AddMarten(options =>
            {
                options.Connection("host=localhost;database=mydatabase;username=admin;password=secret");
                options.UseSystemTextJsonForSerialization();
                options.AutoCreateSchemaObjects = AutoCreate.All;
                
                // forces all GUIDs for IDs (instead of strings)
                options.Events.StreamIdentity = StreamIdentity.AsGuid;
                
                // add projections
                options.Projections.Add<TimeOffRequestProjection>(ProjectionLifecycle.Inline);
            });

            var serviceProvider = services.BuildServiceProvider();
            var documentStore = serviceProvider.GetRequiredService<IDocumentStore>();

            repository = new PostgreSqlTimeOffRequestRepository(documentStore);
        }

        [Fact]
        public async Task Save_SavesStuff()
        {
            var timeOffRequest = new TimeOffRequest(TimeOffRequestId.New(), DateTime.Now, DateTime.Now.AddDays(1));
            timeOffRequest.Cancel();

            await repository.Save(timeOffRequest, CancellationToken.None);
        }

        [Fact]
        public async Task Hydrate_HydratesStuff()
        {
            var id = new TimeOffRequestId(spikeId);

            var timeOffRequest = await repository.Hydrate(id, null, CancellationToken.None);

            timeOffRequest.Should().NotBeNull();
        }

        [Fact]
        public async Task Get_GetsStuff()
        {
            var id = new TimeOffRequestId(spikeId);

            var timeOffRequest = await repository.Get(id, CancellationToken.None);

            timeOffRequest.Should().NotBeNull();
        }
    }
}
