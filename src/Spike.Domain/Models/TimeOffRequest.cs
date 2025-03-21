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
        public TimeOffRequestStatus Status { get; private set; }

        private TimeOffRequest() { }

        public TimeOffRequest(TimeOffRequestId id, 
            DateTime start, 
            DateTime end, 
            TimeOffRequestStatus status = TimeOffRequestStatus.Pending)
        {
            var @event = new TimeOffRequestCreated
            {
                Id = id,
                Start = start,
                End = end,
                Status = status
            };

            var timeOffRequest = new TimeOffRequest();

            Apply(@event);
            AddUncommittedEvent(@event);
        }

        public void Cancel()
        {
            var @event = new TimeOffRequestCancelled();
            
            Apply(@event);
            AddUncommittedEvent(@event);
        }

        private void Apply(TimeOffRequestCreated @event)
        {
            Id = @event.Id;
            Start = @event.Start;
            End = @event.End;
            Status = @event.Status;
            Version++;
        }

        private void Apply(TimeOffRequestCancelled @event)
        {
            Status = TimeOffRequestStatus.Cancelled;
            Version++;
        }
    }

    public enum TimeOffRequestStatus
    {
        Pending,
        Approved,
        Denied,
        Cancelled
    }
}
