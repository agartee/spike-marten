using Marten;
using Spike.Domain.Events;
using Spike.Domain.Models;
using Spike.Domain.Services;

namespace Spike.Persistence.Marten.Services
{
    public class MartenTimeOffRequestRepository : ITimeOffRequestRepository
    {
        private readonly IDocumentStore documentStore;

        public MartenTimeOffRequestRepository(IDocumentStore documentStore)
        {
            this.documentStore = documentStore;
        }

        public async Task Save(TimeOffRequest timeOffRequest, CancellationToken cancellationToken)
        {
            using (var session = documentStore.LightweightSession())
            {
                foreach (var @event in timeOffRequest.GetUncommittedEvents())
                {
                    if(@event is TimeOffRequestCreated timeOffRequestCreated)
                        session.Events.StartStream<TimeOffRequest>(timeOffRequest.Id.Value, @event);
                    else
                        session.Events.Append(timeOffRequest.Id.Value, @event);
                }

                await session.SaveChangesAsync();
                timeOffRequest.ClearUncommittedEvents();
            }
        }
    }
}
