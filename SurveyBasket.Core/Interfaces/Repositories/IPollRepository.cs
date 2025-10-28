using SurveyBasket.Contracts.Polls;
using SurveyBasket.Core.Entities;

namespace SurveyBasket.Core.Interfaces.Repositories
{
    public interface IPollRepository : IGenericRepository<Poll>
    {
        Task<bool> IsExistingTitleAsync(string title, CancellationToken cancellationToken);
        Task<bool> IsExistingTitleWithDifferentIdAsync(int id, string title, CancellationToken cancellationToken);
        Task<bool> IsExistingPoll(int id, CancellationToken cancellationToken);
        Task<bool> IsRunningPoll(int id, CancellationToken cancellationToken);
        Task<IEnumerable<PollResponse>> GetCurrentAsync(CancellationToken cancellationToken);
    }
}
