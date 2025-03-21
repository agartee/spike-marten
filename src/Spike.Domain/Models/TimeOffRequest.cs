using Spike.Domain.Events;
using System.Text.Json.Serialization;

namespace Spike.Domain.Models
{
    public record TimeOffRequest
    {
        #region Aggregate Base Behaviors

        public long Version { get; protected set; }

        [JsonIgnore] 
        private readonly List<object> _uncommittedEvents = [];

        public IEnumerable<object> GetUncommittedEvents() => _uncommittedEvents;
        public void ClearUncommittedEvents() => _uncommittedEvents.Clear();
        protected void AddUncommittedEvent(object @event) => _uncommittedEvents.Add(@event);

        #endregion

        public TimeOffRequestId? Id { get; private set; }
        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }

        public TimeOffRequest(TimeOffRequestId id, DateTime start, DateTime end)
        {
            var @event = new TimeOffRequestCreated
            {
                Id = id,
                Start = start,
                End = end
            };

            Apply(@event);
            AddUncommittedEvent(@event);
        }

        #region I don't like this... (Marten work-arounds for DDD behavior)

        [JsonConstructor]
        private TimeOffRequest() { }

        #endregion

        public void Apply(TimeOffRequestCreated @event)
        {
            Id = @event.Id;
            Start = @event.Start;
            End = @event.End;

            Version++;
        }
    }
}
