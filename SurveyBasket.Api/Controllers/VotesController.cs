using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Api.Extensions;
using SurveyBasket.Contracts.Abstractions;
using SurveyBasket.Contracts.Votes;
using SurveyBasket.Core.Interfaces.Services;
using System.Security.Claims;

namespace SurveyBasket.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class VotesController(IQuestionService questionService, IVoteService voteService) : ControllerBase
{
    private readonly IQuestionService _questionService = questionService;
    private readonly IVoteService _voteService = voteService;

    [HttpGet("GetAvailableQuestions/pollId/{pollId}")]
    public async Task<IActionResult> GetAvailableQuestionsForVote([FromRoute] int pollId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();

        var result = await _questionService.GellAvailableQuestionsAsync(pollId, userId!, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost("SubmitVote")]
    public async Task<IActionResult> SubmitVote([FromBody] VoteRequest voteRequest, CancellationToken cancellationToken) 
    {
        var userId = User.GetUserId();

        var result = await _voteService.SubmitVoteAsync(userId!, voteRequest, cancellationToken);

        return result.IsSuccess ? Ok(result.IsSuccess) : result.ToProblem();
    }

}