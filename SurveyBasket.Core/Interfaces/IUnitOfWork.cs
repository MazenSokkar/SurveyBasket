using SurveyBasket.Core.Entities;

namespace SurveyBasket.Core.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<Poll> Polls {  get; }

    Task<int> Complete(CancellationToken cancellationToken = default);
}
