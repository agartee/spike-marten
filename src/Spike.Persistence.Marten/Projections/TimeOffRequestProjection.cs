using Marten.Events.Aggregation;
using Spike.Domain.Events;
using Spike.Domain.Models;

namespace Spike.Persistence.Marten.Projections
{
    public class TimeOffRequestProjection : SingleStreamProjection<TimeOffRequest>
    {
    }
}
