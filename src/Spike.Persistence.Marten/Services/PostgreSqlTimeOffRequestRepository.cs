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

        public async Task<TimeOffRequest> Hydrate(TimeOffRequestId id, int? version, CancellationToken cancellationToken)
        {
            await using var session = await store.LightweightSerializableSessionAsync(token: cancellationToken);
            var result = await session.Events.AggregateStreamAsync<TimeOffRequest>(id.Value, version ?? 0, token: cancellationToken);

            return result ?? throw new InvalidOperationException($"No aggregate found with ID {id}.");
        }

        public async Task<TimeOffRequest> Get(TimeOffRequestId id, CancellationToken cancellationToken)
        {
            await using var session = await store.LightweightSerializableSessionAsync(token: cancellationToken);
            
            var result = await session.LoadAsync<TimeOffRequest>(id, token: cancellationToken);

            return result ?? throw new InvalidOperationException($"No projection found with ID {id}.");
        }
    }
}
