using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SurveyBasket.Contracts.Requests;
using SurveyBasket.Contracts.Responses;
using SurveyBasket.Core.Entities;
using SurveyBasket.Core.Interfaces;

namespace SurveyBasket.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public async Task<IActionResult> AddPoll([FromBody] AddPollRequest request, 
            [FromServices] IValidator<AddPollRequest> validator,
            CancellationToken cancellationToken)
        {
            var mappedRequest = request.Adapt<Poll>();

            var newPoll = await _pollService.AddAsync(mappedRequest, cancellationToken);

            return CreatedAtAction(nameof(GetPollById), new { id = newPoll.Id} ,newPoll);
        }

        //[HttpPut("{id}")]
        //public IActionResult UpdatePoll([FromRoute] int id, [FromBody] AddPollRequest poll)
        //{
        //    var isUpdated = _pollService.Update(id, poll.Adapt<Poll>());

        //    return isUpdated? Ok(isUpdated) : NotFound();
        //}

        //[HttpDelete("{id}")]
        //public IActionResult DeletePoll([FromRoute] int id) 
        //{
        //    var isDeleted = _pollService.Delete(id);
            
        //    return isDeleted? Ok(isDeleted) : NotFound();
        //}
    }
}
