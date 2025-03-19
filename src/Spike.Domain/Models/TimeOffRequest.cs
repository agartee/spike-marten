using Spike.Domain.Commands;
using Spike.Domain.Events;

namespace Spike.Domain.Models
{
    public record TimeOffRequest
    {
        // note: only needs to be nullable when using an identity model
        public TimeOffRequestId? Id { get; private set; }
        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }

        public void Apply(TimeOffRequestCreated @event)
        {
            Id = @event.Id;
            Start = @event.Start;
            End = @event.End;
        }
    }
}
