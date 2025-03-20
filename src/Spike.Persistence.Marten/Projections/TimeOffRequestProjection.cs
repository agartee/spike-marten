using Marten.Events.Aggregation;
using Spike.Domain.Events;
using Spike.Domain.Models;

namespace Spike.Persistence.Marten.Projections
{
    // note: the existance of this class and its registration with the Marten projections options
    // is all that's needed for the projection table to be created.
    public class TimeOffRequestProjection : SingleStreamProjection<TimeOffRequest>
    {
    }
}
