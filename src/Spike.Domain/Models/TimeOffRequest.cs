using Spike.Domain.Commands;
using Spike.Domain.Events;

namespace Spike.Domain.Models
{
    public record TimeOffRequest
    {
        // note: added so that we can push Marten interactions into a repository
        // note: we can also extract these to a base class
        private readonly List<object> uncommittedEvents = new();
        public IEnumerable<object> GetUncommittedEvents() => uncommittedEvents.AsReadOnly();
        public void ClearUncommittedEvents() => uncommittedEvents.Clear();

        // note: we don't want properties to be directly updated, only updated via events

        // note: ID field only needs to be nullable when using an identity model
        public TimeOffRequestId Id { get; private set; }
        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }

        public static TimeOffRequest Create(DateTime start, DateTime end)
        {
            var @event = new TimeOffRequestCreated
            {
                Id = TimeOffRequestId.New(),
                Start = start,
                End = end
            };

            var timeOffRequest = new TimeOffRequest();

            timeOffRequest.Apply(@event);
            timeOffRequest.uncommittedEvents.Add(@event);

            return timeOffRequest;
        }

        // note: this will get called when fetching an entity via session.Events.AggregateStreamAsync
        // note: this is called via reflection, but there may be some reflection-mapping caching going on in Marten
        public void Apply(TimeOffRequestCreated @event)
        {
            Id = @event.Id;
            Start = @event.Start;
            End = @event.End;
        }
    }
}
