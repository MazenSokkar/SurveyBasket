using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Contracts.Abstractions;
using SurveyBasket.Contracts.Polls;
using SurveyBasket.Core.Entities;
using SurveyBasket.Core.Interfaces.Services;

namespace SurveyBasket.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PollsController(IPollService pollService) : ControllerBase
    {

        private readonly IPollService _pollService = pollService;

        [HttpGet]
        public async Task<IActionResult> GetAllPolls(CancellationToken cancellationToken)
        {
            var polls = await _pollService.GetAllAsync(cancellationToken);

            return polls.IsSuccess ? Ok(polls.Value)
                : polls.ToProblem();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetPollById([FromRoute] int id, CancellationToken cancellationToken)
        {
            var poll = await _pollService.GetByIdAsync(id, cancellationToken);

            return poll.IsSuccess ? Ok(poll.Value)
                : poll.ToProblem();
        }

        [HttpPost("")]
        public async Task<IActionResult> AddPoll([FromBody] PollRequest request, 
            [FromServices] IValidator<PollRequest> validator,
            CancellationToken cancellationToken)
        {
            var newPoll = await _pollService.AddAsync(request, cancellationToken);

            return newPoll.IsSuccess ? CreatedAtAction(nameof(GetPollById), new { id = newPoll.Value.Id }, newPoll.Value)
                : newPoll.ToProblem();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePoll([FromRoute] int id, [FromBody] PollRequest poll, CancellationToken cancellationToken)
        {
            var isUpdated = await _pollService.UpdateAsync(id, poll, cancellationToken);

            return isUpdated.IsSuccess ? Ok(isUpdated.IsSuccess)
                : isUpdated.ToProblem();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePoll([FromRoute] int id)
        {
            var isDeleted = await _pollService.DeleteAsync(id);

            return isDeleted.IsSuccess ? Ok(isDeleted.IsSuccess)
                : isDeleted.ToProblem();
        }

        [HttpPut("{id}/ChangePublishStatus")]
        public async Task<IActionResult> ChangePublishStatus([FromRoute] int id, CancellationToken cancellationToken)
        {
            var isToggled = await _pollService.TogglePublishStatusAsync(id, cancellationToken);

            return isToggled.IsSuccess ? Ok(isToggled.IsSuccess)
                : isToggled.ToProblem();
        }
    }
}
