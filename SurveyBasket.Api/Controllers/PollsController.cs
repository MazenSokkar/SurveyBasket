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
        public IActionResult GetAllPolls()
        {
            return Ok(_pollService.GetAll().Adapt<IEnumerable<Poll>>());
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetPollById([FromRoute] int id)
        {
            var poll = _pollService.GetById(id).Adapt<PollResponse>();

            return poll is null ? NotFound() : Ok(poll);
        }

        [HttpPost("")]
        public IActionResult AddPoll([FromBody] AddPollRequest request, [FromServices] IValidator<AddPollRequest> validator)
        {
            var newPoll = _pollService.Add(request.Adapt<Poll>());

            return CreatedAtAction(nameof(GetPollById), new { id = newPoll.Id} ,newPoll);
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePoll([FromRoute] int id, [FromBody] AddPollRequest poll)
        {
            var isUpdated = _pollService.Update(id, poll.Adapt<Poll>());

            return isUpdated? Ok(isUpdated) : NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePoll([FromRoute] int id) 
        {
            var isDeleted = _pollService.Delete(id);
            
            return isDeleted? Ok(isDeleted) : NotFound();
        }
    }
}
