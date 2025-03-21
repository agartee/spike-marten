using Spike.Domain.Models;

namespace Spike.Domain.Services
{
    public interface ITimeOffRequestRepository
    {
        Task Save(TimeOffRequest timeOffRequest, CancellationToken cancellationToken);
        Task<TimeOffRequest> Hydrate(TimeOffRequestId id, int? version, CancellationToken cancellationToken);
        Task<TimeOffRequestInfo> Get(TimeOffRequestId id, CancellationToken cancellationToken);
    }
}
