using SurveyBasket.Core.Entities;

namespace SurveyBasket.Core.Interfaces
{
    public interface IPollService
    {
        Task<IEnumerable<Poll>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<Poll?> GetByIdAsync(int Id, CancellationToken cancellationToken = default);

        Task<Poll> AddAsync(Poll poll, CancellationToken cancellationToken = default);

        Task<bool> UpdateAsync(int id, Poll poll, CancellationToken cancellationToken = default);

        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    
        Task<bool> TogglePublishStatusAsync(int id, CancellationToken cancellationToken = default);
    }
}
