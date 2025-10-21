using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

            var response = polls.Adapt<IEnumerable<PollResponse>>();

            return Ok(response);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetPollById([FromRoute] int id, CancellationToken cancellationToken)
        {
            var poll = await _pollService.GetByIdAsync(id, cancellationToken);
            
            var response = poll.Adapt<PollResponse>();

            return response is null ? NotFound() : Ok(response);
        }

        [HttpPost("")]
        public async Task<IActionResult> AddPoll([FromBody] PollRequest request, 
            [FromServices] IValidator<PollRequest> validator,
            CancellationToken cancellationToken)
        {
            var mappedRequest = request.Adapt<Poll>();

            var newPoll = await _pollService.AddAsync(mappedRequest, cancellationToken);

            return CreatedAtAction(nameof(GetPollById), new { id = newPoll.Id} ,newPoll);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePoll([FromRoute] int id, [FromBody] PollRequest poll, CancellationToken cancellationToken)
        {
            var isUpdated = await _pollService.UpdateAsync(id, poll.Adapt<Poll>(), cancellationToken);

            return isUpdated ? Ok(isUpdated) : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePoll([FromRoute] int id, CancellationToken cancellationToken)
        {
            var isDeleted = await _pollService.DeleteAsync(id);

            return isDeleted ? Ok(isDeleted) : NotFound();
        }

        [HttpPut("{id}/ChangePublishStatus")]
        public async Task<IActionResult> ChangePublishStatus([FromRoute] int id, CancellationToken cancellationToken)
        {
            var isToggled = await _pollService.TogglePublishStatusAsync(id, cancellationToken);

            return isToggled ? Ok(isToggled) : NotFound();
        }
    }
}
