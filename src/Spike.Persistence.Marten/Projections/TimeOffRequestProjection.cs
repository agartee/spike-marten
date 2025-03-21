using Marten.Events.Aggregation;
using Spike.Domain.Events;
using Spike.Domain.Models;

namespace Spike.Persistence.Marten.Projections
{
    public class TimeOffRequestProjection : SingleStreamProjection<TimeOffRequestInfo>
    {
        public TimeOffRequestInfo Create(TimeOffRequestCreated @event)
        {
            return new TimeOffRequestInfo
            {
                Id = @event.Id,
                Start = @event.Start,
                End = @event.End
            };
        }

        public TimeOffRequestInfo Apply(TimeOffRequestCancelled @event, TimeOffRequestInfo timeOffRequest)
        {
            return new TimeOffRequestInfo
            {
                Id = timeOffRequest.Id,
                Start = timeOffRequest.Start,
                End = timeOffRequest.End,
                Status = TimeOffRequestStatus.Cancelled
            };
        }
    }
}
