using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Contracts.Abstractions;
using SurveyBasket.Contracts.Questions;
using SurveyBasket.Core.Entities;
using SurveyBasket.Core.Interfaces.Services;

namespace SurveyBasket.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QuestionsController(IQuestionService questionService) : ControllerBase
    {
        private readonly IQuestionService _questionService = questionService;

        [HttpPost("AddQuestion")]
        public async Task<IActionResult> AddQuestion([FromBody] QuestionRequest questionRequest, CancellationToken cancellationToken)
        {
            var result = await _questionService.AddAsync(questionRequest, cancellationToken);
            return result.IsSuccess ? CreatedAtAction(
                actionName: nameof(GetQuestionById),
                controllerName: "Questions",
                routeValues: new { pollId = result.Value.PollId, questionId = result.Value.Id },
                value: result.Value
            )
            : result.ToProblem();
        }

        [HttpGet("GetQuestionsByPollId/{id}")]
        public async Task<IActionResult> GetPollQuestions([FromRoute] int id, CancellationToken cancellationToken)
        { 
            var result = await _questionService.GellQuestionsByPollIdAsync(id, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpGet("PollId/{pollId}/QuestionId/{questionId}")]
        public async Task<IActionResult> GetQuestionById([FromRoute] int pollId, [FromRoute] int questionId, CancellationToken cancellationToken) 
        { 
            var result = await _questionService.GetQuestionsById(pollId,questionId, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem(); 
        }

        [HttpPut("ChangeActiveStatus/PollId/{pollId}/QuestionId/{questionId}")]
        public async Task<IActionResult> ChangeActiveStatus([FromRoute] int pollId, [FromRoute] int questionId, CancellationToken cancellationToken)
        {
            var result = await _questionService.ToggleActiveStatusAsync(pollId, questionId, cancellationToken);

            return result.IsSuccess ? Ok(result.IsSuccess) : result.ToProblem();
        }

        [HttpPut("UpdateQuestion/QuestionId/{questionId}")]
        public async Task<IActionResult> UpdateQuestion([FromRoute] int questionId, [FromBody] QuestionRequest questionRequest, CancellationToken cancellationToken)
        {
            var result = await _questionService.UpdateAsync(questionId, questionRequest, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }
    }
}
