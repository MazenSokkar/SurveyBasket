using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Api.Mapping;
using SurveyBasket.Contracts.Requests;
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
            return Ok(_pollService.GetAll().MapToResponse());
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetPollById([FromRoute] int id)
        {
            var poll = _pollService.GetById(id);

            return poll is null ? NotFound() : Ok(poll.MapToResponse());
        }

        [HttpPost("")]
        public IActionResult AddPoll([FromBody] AddPollRequest request)
        {
            var newPoll = _pollService.Add(request.MapToPoll());

            return CreatedAtAction(nameof(GetPollById), new { id = newPoll.Id} ,newPoll);
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePoll([FromRoute] int id, [FromBody] AddPollRequest poll)
        {
            var isUpdated = _pollService.Update(id, poll.MapToPoll());

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
