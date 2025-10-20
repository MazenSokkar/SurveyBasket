using SurveyBasket.Core.Entities;

namespace SurveyBasket.Core.Interfaces.Repositories
{
    public interface IPollRepository : IGenericRepository<Poll>
    {
        Task<bool> UpdateAsync(int id, Poll poll, CancellationToken cancellationToken = default);
        Task<bool> TogglePublishStatusAsync(int id, CancellationToken cancellationToken = default);
    }
}
