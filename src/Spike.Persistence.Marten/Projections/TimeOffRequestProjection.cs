using Marten.Events.Aggregation;
using Spike.Domain.Events;
using Spike.Domain.Models;

namespace Spike.Persistence.Marten.Projections
{
    public class TimeOffRequestProjection : SingleStreamProjection<TimeOffRequest>
    {
        public TimeOffRequest Create(TimeOffRequestCreated @event)
        {
            return new TimeOffRequest(@event.Id, @event.Start, @event.End);
        }
    }
}
