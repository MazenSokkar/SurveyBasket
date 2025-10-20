using SurveyBasket.Core.Entities;

namespace SurveyBasket.Core.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    Task<int> Complete(CancellationToken cancellationToken = default);
}
