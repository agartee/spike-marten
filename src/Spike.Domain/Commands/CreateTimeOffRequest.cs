using MediatR;
using Spike.Domain.Models;
using Spike.Domain.Services;

namespace Spike.Domain.Commands
{
    public record CreateTimeOffRequest : IRequest<TimeOffRequest>
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }

    public class CreateTimeOffRequestHandler : IRequestHandler<CreateTimeOffRequest, TimeOffRequest>
    {
        private readonly ITimeOffRequestRepository timeOffRequestRepository;

        public CreateTimeOffRequestHandler(ITimeOffRequestRepository timeOffRequestRepository)
        {
            this.timeOffRequestRepository = timeOffRequestRepository;
        }

        public async Task<TimeOffRequest> Handle(CreateTimeOffRequest request, CancellationToken cancellationToken)
        {
            // note: validation should happen here, before the event is created
            // should we use the Guard pattern?

            var timeOffRequest = new TimeOffRequest(TimeOffRequestId.New(), request.Start, request.End);

            await timeOffRequestRepository.Save(timeOffRequest, cancellationToken);

            return timeOffRequest;
        }
    }
}
