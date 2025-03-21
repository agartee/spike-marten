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
        [Fact]
        public async Task Save_SavesStuff()
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

            // our stuff!
            var repository = new PostgreSqlTimeOffRequestRepository(documentStore);

            var timeOffRequest = new TimeOffRequest(TimeOffRequestId.New(), DateTime.Now, DateTime.Now.AddDays(1));

            await repository.Save(timeOffRequest, CancellationToken.None);
        }
    }
}
