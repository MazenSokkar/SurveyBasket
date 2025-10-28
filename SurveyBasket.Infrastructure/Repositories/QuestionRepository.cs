using Mapster;
using Microsoft.EntityFrameworkCore;
using SurveyBasket.Contracts.Answers;
using SurveyBasket.Contracts.Questions;
using SurveyBasket.Core.Entities;
using SurveyBasket.Core.Interfaces.Repositories;
using SurveyBasket.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SurveyBasket.Infrastructure.Repositories
{
    public class QuestionRepository(IGenericRepository<Question> genericRepository, ApplicationDbContext context) : IQuestionRepository
    {
        private readonly IGenericRepository<Question> _genericRepository = genericRepository;
        private readonly ApplicationDbContext _context = context;

        public async Task<Question> AddAsync(Question entity, CancellationToken cancellationToken = default)
        {
            return await _genericRepository.AddAsync(entity, cancellationToken);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _genericRepository.DeleteAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<Question>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<Question?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsExistingQuestionForAdd(int pollId, string questionContent, CancellationToken cancellationToken)
            => await _context.Questions.AnyAsync(x => x.Content == questionContent && x.PollId == pollId, cancellationToken);

        public async Task<bool> IsExistingQuestionForUpdate(int pollId, int questionId, string questionContent, CancellationToken cancellationToken)
            => await _context.Questions
                .AnyAsync(x => x.PollId == pollId
                               && x.Id != questionId
                               && x.Content.ToLower() == questionContent.ToLower(),
                          cancellationToken);

        public async Task<IEnumerable<QuestionResponse>> GellQuestionsByPollId(int pollId, CancellationToken cancellationToken = default)
            =>  await _context.Questions
                .Where(q => q.PollId == pollId)
                .Include(x => x.Answers)
                .Select(q => new QuestionResponse
                (
                    q.Id,
                    q.PollId,
                    q.IsActive,
                    q.Content,
                    q.Answers.Select(a => new AnswerResponse(a.Id, a.Content))
                ))
                .AsNoTracking()
                .ToListAsync(cancellationToken);

        public async Task<Question?> GetQuestionsById(int pollId, int questionId, CancellationToken cancellationToken = default)
        {
            return await _context.Questions
                .Include(q => q.Answers)
                .SingleOrDefaultAsync(q => q.PollId == pollId && q.Id == questionId, cancellationToken);
        }

        public async Task<IEnumerable<QuestionResponse>> GetAvailableQuestions(int pollId, string UserId, CancellationToken cancellationToken)
        {
            return await _context.Questions
                .Where(x => x.PollId == pollId && x.IsActive)
                .Include (x => x.Answers)
                .Select(q => new QuestionResponse(
                    q.Id,
                    q.PollId,
                    q.IsActive,
                    q.Content,
                    q.Answers.Where(a => a.IsActive).Select(a => new AnswerResponse(a.Id, a.Content))
                ))
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<int>> GetPollQuestionsIds(int pollId, CancellationToken cancellationToken)
            => await _context.Questions
                .Where(x => x.PollId == pollId && x.IsActive)
                .Select(x => x.Id)
                .ToListAsync(cancellationToken);
    }
}
