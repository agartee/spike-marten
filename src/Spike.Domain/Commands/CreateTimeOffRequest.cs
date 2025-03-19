using Marten;
using MediatR;
using Spike.Domain.Events;
using Spike.Domain.Models;

namespace Spike.Domain.Commands
{
    public record CreateTimeOffRequest : IRequest<TimeOffRequest>
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }

    public class CreateTimeOffRequestHandler : IRequestHandler<CreateTimeOffRequest, TimeOffRequest>
    {
        private readonly IDocumentStore documentStore;

        public CreateTimeOffRequestHandler(IDocumentStore documentStore)
        {
            this.documentStore = documentStore;
        }

        public async Task<TimeOffRequest> Handle(CreateTimeOffRequest request, CancellationToken cancellationToken)
        {
            var id = TimeOffRequestId.New();
            var @event = new TimeOffRequestCreated
            {
                Id = id,
                Start = request.Start,
                End = request.End
            };

            using (var session = documentStore.LightweightSession())
            {
                session.Events.StartStream<TimeOffRequest>(@event);
                await session.SaveChangesAsync();
            }

            // note: I get null results when querying for my newly created streams and projections
            var result = new TimeOffRequest();
            result.Apply(@event);

            return result;
        }

        public async Task Testing()
        {
            var id = new TimeOffRequestId(new Guid("0195afdb-ecda-40f5-8ac2-3f399555c697"));
            using var session = documentStore.LightweightSession();

            // the stream ID referenced here cannot be the identity model type
            var timeOffRequest = await session.Events.AggregateStreamAsync<TimeOffRequest>(id.Value);

            //var timeOffRequest2 = await session.LoadAsync<TimeOffRequest>(id);
        }
    }
}
