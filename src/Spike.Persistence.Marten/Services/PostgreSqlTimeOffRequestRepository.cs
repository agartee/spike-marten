using Marten;
using Spike.Domain.Models;
using Spike.Domain.Services;

namespace Spike.Persistence.Marten.Services
{
    public sealed class PostgreSqlTimeOffRequestRepository : ITimeOffRequestRepository
    {
        private readonly IDocumentStore store;

        public PostgreSqlTimeOffRequestRepository(IDocumentStore store)
        {
            this.store = store;
        }

        public async Task Save(TimeOffRequest timeOffRequest, CancellationToken cancellationToken)
        {
            await using var session = await store.LightweightSerializableSessionAsync(cancellationToken);

            var events = timeOffRequest.GetUncommittedEvents();
            session.Events.Append(timeOffRequest.Id!.Value.Value, timeOffRequest.Version, events); // eww, cleanup!

            await session.SaveChangesAsync(cancellationToken);
            timeOffRequest.ClearUncommittedEvents();
        }

        //public async Task<TAggregate> LoadAsync<TAggregate>(string id, int? version, CancellationToken cancellationToken) 
        //    where TAggregate : AggregateBase
        //{
        //    await using var session = await store.LightweightSerializableSessionAsync(token: cancellationToken);
        //    var aggregate = await session.Events.AggregateStreamAsync<TAggregate>(id, version ?? 0, token: cancellationToken);

        //    return aggregate ?? throw new InvalidOperationException($"No aggregate found with ID {id}.");
        //}
    }
}
