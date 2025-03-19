namespace Spike.Domain.Models
{
    public readonly record struct TimeOffRequestId(Guid Value)
    {
        public static TimeOffRequestId New() => new(Guid.CreateVersion7());

        public override string ToString() => Value.ToString();
    }
}
