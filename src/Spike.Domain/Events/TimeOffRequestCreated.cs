using Spike.Domain.Models;

namespace Spike.Domain.Events
{
    public record TimeOffRequestCreated
    {
        public required TimeOffRequestId Id { get; init; }
        public required DateTime Start { get; init; }
        public required DateTime End { get; init; }
    }
}
