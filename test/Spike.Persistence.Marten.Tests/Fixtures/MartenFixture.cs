using Marten;
using Marten.Events;
using Marten.Events.Projections;
using Microsoft.Extensions.DependencyInjection;
using Spike.Persistence.Marten.Projections;
using Weasel.Core;

namespace Spike.Persistence.Marten.Tests
{
    public class MartenFixture : IDisposable
    {
        private readonly ServiceProvider serviceProvider;

        public MartenFixture()
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

            serviceProvider = services.BuildServiceProvider();
        }

        public IDocumentStore CreateDocumentStore()
        {
            return serviceProvider.GetRequiredService<IDocumentStore>();
        }

        public void Dispose()
        {
            serviceProvider.Dispose();
        }
    }

    [CollectionDefinition("Marten collection")]
    public class MartenCollection : ICollectionFixture<MartenFixture>
    {
    }
}
