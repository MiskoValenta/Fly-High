using FlyHigh.DTOs.TeamDTOs;
using FlyHigh.DTOs.TeamMemberDTOs;
using FlyHigh.Models.TeamModels;
using FlyHigh.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlyHigh.Controllers
{
    [ApiController]
    [Route("Teams/[controller]")]
    [Authorize]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService _teamService;

        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        // GET: Teams/Team
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamSummaryDto>>> GetTeams()
        {
            var currentUserId = User.FindFirst("id")?.Value;
            var teams = await _teamService.GetUserTeamsAsync(currentUserId);

            var teamSummaries = teams.Select(t => new TeamSummaryDto
            {
                Id = t.Id,
                Name = t.Name,
                City = t.City,
                LogoUrl = t.LogoUrl,
                IsActive = t.IsActive,
                CreatedAt = t.CreatedAt
            });

            return Ok(teamSummaries);
        }

        // GET: Teams/Team/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TeamDetailDto>> GetTeam(int id)
        {
            // Zde by měl být rozšířený service pro získání detailu týmu
            // Pro zjednodušení použiju základní
            return Ok(new TeamDetailDto { Id = id, Name = "Placeholder" });
        }

        // POST: Teams/Team
        [HttpPost]
        public async Task<ActionResult<TeamSummaryDto>> CreateTeam(CreateTeamDto createTeamDto)
        {
            var currentUserId = User.FindFirst("id")?.Value;
            var team = await _teamService.CreateTeamAsync(createTeamDto, currentUserId);

            var teamSummary = new TeamSummaryDto
            {
                Id = team.Id,
                Name = team.Name,
                City = team.City,
                LogoUrl = team.LogoUrl,
                IsActive = team.IsActive,
                CreatedAt = team.CreatedAt
            };

            return CreatedAtAction(nameof(GetTeam), new { id = team.Id }, teamSummary);
        }

        // POST: Teams/Team/5/invite
        [HttpPost("{id}/invite")]
        public async Task<ActionResult> InviteToTeam(int id, InviteToTeamDto inviteDto)
        {
            // Zde by bylo potřeba najít uživatele podle emailu
            // Pro zjednodušení předpokládám, že máme UserId
            var success = await _teamService.InviteUserToTeamAsync(id, "userId", Enum.Parse<TeamRole>(inviteDto.Role));

            if (!success)
                return BadRequest("Nepodařilo se pozvat uživatele do týmu");

            return Ok();
        }

        // POST: Teams/Team/5/join
        [HttpPost("{id}/join")]
        public async Task<ActionResult> JoinTeam(int id)
        {
            var currentUserId = User.FindFirst("id")?.Value;
            var success = await _teamService.AcceptTeamInvitationAsync(id, currentUserId);

            if (!success)
                return BadRequest("Nepodařilo se připojit k týmu");

            return Ok();
        }

        // PUT: api/teams/5/members/userId/role
        [HttpPut("{teamId}/members/{userId}/role")]
        public async Task<ActionResult> UpdateMemberRole(int teamId, string userId, UpdateTeamMemberDto updateDto)
        {
            var success = await _teamService.UpdateTeamRoleAsync(teamId, userId, Enum.Parse<TeamRole>(updateDto.Role));

            if (!success)
                return BadRequest("Nepodařilo se aktualizovat roli");

            return Ok();
        }

        // DELETE: api/teams/5/members/userId
        [HttpDelete("{teamId}/members/{userId}")]
        public async Task<ActionResult> RemoveMember(int teamId, string userId)
        {
            var success = await _teamService.RemoveUserFromTeamAsync(teamId, userId);

            if (!success)
                return BadRequest("Nepodařilo se odebrat člena");

            return Ok();

        }
    }
}
