using Spike.Domain.Models;

namespace Spike.Domain.Services
{
    public interface ITimeOffRequestRepository
    {
        Task Save(TimeOffRequest timeOffRequest, CancellationToken cancellationToken);
    }
}
