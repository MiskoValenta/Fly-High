using FlyHigh.DTOs.PollDTOs;
using FlyHigh.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlyHigh.Controllers
{
    [ApiController]
    [Route("Poll/[controller]")]
    [Authorize]
    public class PollsController : ControllerBase
    {
        private readonly IPollService _pollService;

        public PollsController(IPollService pollService)
        {
            _pollService = pollService;
        }

        // GET: Poll/polls?teamId=5
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PollSummaryDto>>> GetPolls(
            [FromQuery] int teamId,
            [FromQuery] bool activeOnly = true)
        {
            var polls = await _pollService.GetTeamPollsAsync(teamId, activeOnly);
            var currentUserId = User.FindFirst("id")?.Value;

            var pollSummaries = polls.Select(p => new PollSummaryDto
            {
                Id = p.Id,
                TeamId = p.TeamId,
                Title = p.Title,
                Type = p.Type.ToString(),
                IsActive = p.IsActive,
                ExpiresAt = p.ExpiresAt,
                CreatedAt = p.CreatedAt,
                TotalResponses = p.Responses.Count,
                HasUserResponded = p.Responses.Any(r => r.UserId == currentUserId)
            });

            return Ok(pollSummaries);
        }

        // GET: Poll/polls/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PollDetailDto>> GetPoll(int id)
        {
            // Implementovat získání detailu ankety
            return Ok(new PollDetailDto { Id = id, Title = "Placeholder" });
        }

        // POST: Poll/polls
        [HttpPost]
        public async Task<ActionResult<PollSummaryDto>> CreatePoll(CreatePollDto createPollDto)
        {
            var currentUserId = User.FindFirst("id")?.Value;
            var poll = await _pollService.CreatePollAsync(createPollDto, currentUserId);

            var pollSummary = new PollSummaryDto
            {
                Id = poll.Id,
                TeamId = poll.TeamId,
                Title = poll.Title,
                Type = poll.Type.ToString(),
                IsActive = poll.IsActive,
                ExpiresAt = poll.ExpiresAt,
                CreatedAt = poll.CreatedAt
            };

            return CreatedAtAction(nameof(GetPoll), new { id = poll.Id }, pollSummary);
        }

        // POST: Poll/polls/5/vote
        [HttpPost("{id}/vote")]
        public async Task<ActionResult> VotePoll(int id, VotePollDto voteDto)
        {
            var currentUserId = User.FindFirst("id")?.Value;

            foreach (var optionId in voteDto.SelectedOptionIds)
            {
                var success = await _pollService.VotePollAsync(id, optionId, currentUserId, voteDto.Comment);
                if (!success)
                    return BadRequest("Nepodařilo se zaznamenat hlas");
            }

            return Ok();
        }

        // GET: Poll/polls/5/results
        [HttpGet("{id}/results")]
        public async Task<ActionResult<PollResultDto>> GetPollResults(int id)
        {
            var results = await _pollService.GetPollResultsAsync(id);

            if (results == null)
                return NotFound();

            return Ok(results);
        }

        // PUT: Poll/polls/5/close
        [HttpPut("{id}/close")]
        public async Task<ActionResult> ClosePoll(int id)
        {
            var currentUserId = User.FindFirst("id")?.Value;
            var success = await _pollService.ClosePollAsync(id, currentUserId);

            if (!success)
                return BadRequest("Nepodařilo se uzavřít anketu");

            return Ok();
        }

    }
}
