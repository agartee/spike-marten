namespace Spike.Domain.Models
{
    public class TimeOffRequestInfo
    {
        public TimeOffRequestId? Id { get; init; }
        public DateTime Start { get; init; }
        public DateTime End { get; init; }
        public TimeOffRequestStatus Status { get; init; }
    }
}
