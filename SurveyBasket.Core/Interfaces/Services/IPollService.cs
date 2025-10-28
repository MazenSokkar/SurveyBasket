using SurveyBasket.Contracts.Abstractions;
using SurveyBasket.Contracts.Polls;
using SurveyBasket.Core.Entities;

namespace SurveyBasket.Core.Interfaces.Services
{
    public interface IPollService
    {
        Task<Result<IEnumerable<PollResponse>>> GetAllAsync(CancellationToken cancellationToken = default);
        
        Task<Result<IEnumerable<PollResponse>>> GetCurrentAsync(CancellationToken cancellationToken = default);

        Task<Result<PollResponse>> GetByIdAsync(int Id, CancellationToken cancellationToken = default);

        Task<Result<PollResponse>> AddAsync(PollRequest poll, CancellationToken cancellationToken = default);

        Task<Result> UpdateAsync(int id, PollRequest poll, CancellationToken cancellationToken = default);

        Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default);
    
        Task<Result> TogglePublishStatusAsync(int id, CancellationToken cancellationToken = default);
    }
}
