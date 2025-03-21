using FluentAssertions;
using Marten;
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
        private readonly PostgreSqlTimeOffRequestRepository repository;

        public PostgreSqlTimeOffRequestRepositoryTests()
        {
            var services = new ServiceCollection();

            services.AddMarten(options =>
            {
                options.Connection("host=localhost;database=mydatabase;username=admin;password=secret");
                options.UseSystemTextJsonForSerialization();
                options.AutoCreateSchemaObjects = AutoCreate.All;

                // todo: add projections
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

            await repository.Save(timeOffRequest, CancellationToken.None);
        }

        [Fact]
        public async Task Hydrate_HydratesStuff()
        {
            var id = new TimeOffRequestId(new Guid("0195b8e6-135a-7cd9-a789-c61d1d7b742b"));

            var timeOffRequest = await repository.Hydrate(id, null, CancellationToken.None);

            timeOffRequest.Should().NotBeNull();
        }

        [Fact]
        public async Task Get_GetsStuff()
        {
            var id = new TimeOffRequestId(new Guid("0195b8e6-135a-7cd9-a789-c61d1d7b742b"));

            var timeOffRequest = await repository.Get(id, CancellationToken.None);

            timeOffRequest.Should().NotBeNull();
        }
    }
}
