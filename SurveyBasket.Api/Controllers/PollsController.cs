using Microsoft.AspNetCore.Mvc;
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
            return Ok(_pollService.GetAll());
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetPollById(int id)
        {
            var poll = _pollService.GetById(id);

            return poll is null ? NotFound() : Ok(poll);
        }

        [HttpPost("")]
        public IActionResult AddPoll(Poll request)
        {
            var newPoll = _pollService.Add(request);

            return CreatedAtAction(nameof(GetPollById), new { id = newPoll.Id} ,newPoll);
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePoll(int id, Poll poll)
        {
            var isUpdated = _pollService.Update(id, poll);

            return isUpdated? Ok(isUpdated) : NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePoll(int id) 
        {
            var isDeleted = _pollService.Delete(id);
            
            return isDeleted? Ok(isDeleted) : NotFound();
        }
    }
}
