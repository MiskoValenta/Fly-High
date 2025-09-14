using FlyHigh.Data;
using FlyHigh.DTOs.TrainingDto;
using FlyHigh.Models.TeamModels;
using FlyHigh.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlyHigh.Controllers
{
    [ApiController]
    [Route("Trainings/[controller]")]
    [Authorize]
    public class TrainingsController : ControllerBase
    {
        private readonly ITrainingService _trainingService;

        public TrainingsController(ITrainingService trainingService)
        {
            _trainingService = trainingService;
        }

        // GET: api/trainings?teamId=5
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrainingSummaryDto>>> GetTrainings(
            [FromQuery] int teamId,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            var trainings = await _trainingService.GetTeamTrainingsAsync(teamId, startDate, endDate);

            var trainingSummaries = trainings.Select(t => new TrainingSummaryDto
            {
                Id = t.Id,
                TeamId = t.TeamId,
                Title = t.Title,
                ScheduledDate = t.ScheduledDate,
                StartTime = t.StartTime,
                EndTime = t.EndTime,
                Location = t.Location,
                IsCancelled = t.IsCancelled,
                AttendingCount = t.Attendances.Count(a => a.Status == AttendanceStatus.Attending),
                NotAttendingCount = t.Attendances.Count(a => a.Status == AttendanceStatus.NotAttending),
                MaybeCount = t.Attendances.Count(a => a.Status == AttendanceStatus.Maybe),
                NoResponseCount = t.Attendances.Count(a => a.Status == AttendanceStatus.NoResponse)
            });

            return Ok(trainingSummaries);
        }

        // GET: api/trainings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TrainingDetailDto>> GetTraining(int id)
        {
            // Implementovat získání detailu tréninku
            return Ok(new TrainingDetailDto { Id = id, Title = "Placeholder" });
        }

        // POST: api/trainings
        [HttpPost]
        public async Task<ActionResult<TrainingSummaryDto>> CreateTraining(CreateTrainingDto createTrainingDto)
        {
            var currentUserId = User.FindFirst("id")?.Value;
            var training = await _trainingService.CreateTrainingAsync(createTrainingDto, currentUserId);

            var trainingSummary = new TrainingSummaryDto
            {
                Id = training.Id,
                TeamId = training.TeamId,
                Title = training.Title,
                ScheduledDate = training.ScheduledDate,
                StartTime = training.StartTime,
                EndTime = training.EndTime,
                Location = training.Location,
                IsCancelled = training.IsCancelled
            };

            return CreatedAtAction(nameof(GetTraining), new { id = training.Id }, trainingSummary);
        }

        // POST: api/trainings/5/respond
        [HttpPost("{id}/respond")]
        public async Task<ActionResult> RespondToTraining(int id, RespondToTrainingDto respondDto)
        {
            var currentUserId = User.FindFirst("id")?.Value;
            var status = Enum.Parse<AttendanceStatus>(respondDto.Status);
            var success = await _trainingService.RespondToTrainingAsync(id, currentUserId, status, respondDto.Note);

            if (!success)
                return BadRequest("Nepodařilo se zaznamenat odpověď");

            return Ok();
        }

        // GET: api/trainings/5/attendance
        [HttpGet("{id}/attendance")]
        public async Task<ActionResult<TrainingAttendanceDto>> GetTrainingAttendance(int id)
        {
            var attendance = await _trainingService.GetTrainingAttendanceAsync(id);

            if (attendance == null)
                return NotFound();

            return Ok(attendance);
        }

        // PUT: api/trainings/5/cancel
        [HttpPut("{id}/cancel")]
        public async Task<ActionResult> CancelTraining(int id, [FromBody] string reason)
        {
            var currentUserId = User.FindFirst("id")?.Value;
            var success = await _trainingService.CancelTrainingAsync(id, reason, currentUserId);

            if (!success)
                return BadRequest("Nepodařilo se zrušit trénink");

            return Ok();
        }

    }
}
