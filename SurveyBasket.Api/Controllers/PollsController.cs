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
                : Problem(statusCode: StatusCodes.Status400BadRequest, title: polls.Error.Code, detail: polls.Error.Description);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetPollById([FromRoute] int id, CancellationToken cancellationToken)
        {
            var poll = await _pollService.GetByIdAsync(id, cancellationToken);

            return poll.IsSuccess ? Ok(poll.Value)
                : Problem(statusCode: StatusCodes.Status400BadRequest, title: poll.Error.Code, detail: poll.Error.Description);
        }

        [HttpPost("")]
        public async Task<IActionResult> AddPoll([FromBody] PollRequest request, 
            [FromServices] IValidator<PollRequest> validator,
            CancellationToken cancellationToken)
        {
            var newPoll = await _pollService.AddAsync(request, cancellationToken);

            return newPoll.IsSuccess ? CreatedAtAction(nameof(GetPollById), new { id = newPoll.Value.Id }, newPoll.Value)
                : Problem(statusCode: StatusCodes.Status400BadRequest, title: newPoll.Error.Code, detail: newPoll.Error.Description);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePoll([FromRoute] int id, [FromBody] PollRequest poll, CancellationToken cancellationToken)
        {
            var isUpdated = await _pollService.UpdateAsync(id, poll, cancellationToken);

            return isUpdated.IsSuccess ? Ok(isUpdated.IsSuccess)
                : Problem(statusCode: StatusCodes.Status400BadRequest, title: isUpdated.Error.Code, detail: isUpdated.Error.Description);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePoll([FromRoute] int id)
        {
            var isDeleted = await _pollService.DeleteAsync(id);

            return isDeleted.IsSuccess ? Ok(isDeleted.IsSuccess)
                : Problem(statusCode: StatusCodes.Status400BadRequest, title: isDeleted.Error.Code, detail: isDeleted.Error.Description);
        }

        [HttpPut("{id}/ChangePublishStatus")]
        public async Task<IActionResult> ChangePublishStatus([FromRoute] int id, CancellationToken cancellationToken)
        {
            var isToggled = await _pollService.TogglePublishStatusAsync(id, cancellationToken);

            return isToggled.IsSuccess ? Ok(isToggled.IsSuccess)
                : Problem(statusCode: StatusCodes.Status400BadRequest, title: isToggled.Error.Code, detail: isToggled.Error.Description);
        }
    }
}
