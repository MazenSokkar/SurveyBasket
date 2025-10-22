using Mapster;
using Microsoft.AspNetCore.Mvc.Formatters;
using SurveyBasket.Contracts.Abstractions;
using SurveyBasket.Contracts.Errors;
using SurveyBasket.Contracts.Polls;
using SurveyBasket.Core.Entities;
using SurveyBasket.Core.Interfaces.Repositories;
using SurveyBasket.Core.Interfaces.Services;
using SurveyBasket.Infrastructure.Repositories;

namespace SurveyBasket.Infrastructure.Services
{
    public class PollService(IPollRepository pollRepository, IUnitOfWork unitOfWork) : IPollService
    {
        private readonly IPollRepository _pollRepository = pollRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<IEnumerable<PollResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var polls = await _pollRepository.GetAllAsync(cancellationToken);

            var response = polls.Adapt<IEnumerable<PollResponse>>();

            if (response is null)
                return Result.Failure<IEnumerable<PollResponse>>(PollErrors.NotFoundPolls);
            
            return Result.Success(response);
        }

        public async Task<Result<PollResponse>> GetByIdAsync(int Id, CancellationToken cancellationToken = default)
        { 
            var poll = await _pollRepository.GetByIdAsync(Id, cancellationToken);

            var response = poll.Adapt<PollResponse>();

            if(response is null)
                return Result.Failure<PollResponse>(PollErrors.NotFoundPolls);
            
            return Result.Success(response);
        }

        public async Task<Result<PollResponse>> AddAsync(PollRequest poll, CancellationToken cancellationToken = default)
        {
            var newPoll = await _pollRepository.AddAsync(poll.Adapt<Poll>(), cancellationToken);

            var response = newPoll.Adapt<PollResponse>();
            
            if (response is null)
                return Result.Failure<PollResponse>(PollErrors.NotFoundPolls);

            await _unitOfWork.Complete(cancellationToken);
            
            return Result.Success(response);
        }

        public async Task<Result> UpdateAsync(int id, PollRequest poll, CancellationToken cancellationToken = default)
        {
            var currentPoll = await _pollRepository.GetByIdAsync(id, cancellationToken);

            if (currentPoll is null)
                return Result.Failure(PollErrors.NotFoundPolls);
            
            var newPoll = poll.Adapt<Poll>();
            
            currentPoll.Title = newPoll.Title;
            currentPoll.Summary = newPoll.Summary;
            currentPoll.StartsAt = newPoll.StartsAt;
            currentPoll.EndsAt = newPoll.EndsAt;

            await _unitOfWork.Complete(cancellationToken);
            
            return Result.Success();
        }

        public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            await _pollRepository.DeleteAsync(id);
            await _unitOfWork.Complete(cancellationToken);
            return Result.Success();
        }

        public async Task<Result> TogglePublishStatusAsync(int id, CancellationToken cancellationToken = default)
        {
            var poll = await _pollRepository.GetByIdAsync(id, cancellationToken);

            if (poll is null)
                return Result.Failure(PollErrors.NotFoundPolls);

            poll.IsPublished = !poll.IsPublished;

            await _unitOfWork.Complete(cancellationToken);
            
            return Result.Success();
        }
    }
}
