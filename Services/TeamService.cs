using FlyHigh.Data;
using FlyHigh.DTOs.TeamDTOs;
using FlyHigh.Models;
using FlyHigh.Models.MatchModels;
using FlyHigh.Models.TeamModels;
using FlyHigh.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlyHigh.Services
{
    public class TeamService : ITeamService
    {
        private readonly VolleyballDbContext _context;
        private readonly INotificationService _notificationService;

        public TeamService(VolleyballDbContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        public async Task<bool> AcceptTeamInvitationAsync(int teamId, string userId)
        {
            var membership = new TeamMember
            {
                UserId = userId,
                TeamId = teamId,
                Role = TeamRole.Player
            };

            _context.TeamMembers.Add(membership);
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<IEnumerable<Team>> GetUserTeamsAsync(string userId)
        {
            return await _context.TeamMembers
                .Include(tm => tm.Team)
                .Where(tm => tm.UserId == userId && tm.IsActive)
                .Select(tm => tm.Team)
                .ToListAsync();
        }

        public async Task<Team> CreateTeamAsync(CreateTeamDto teamDto, string creatorUserId)
        {
            var team = new Team
            {
                Name = teamDto.Name,
                Description = teamDto.Description,
                City = teamDto.City,
                ContactEmail = teamDto.ContactEmail,
                ContactPhone = teamDto.ContactPhone
            };

            _context.Teams.Add(team);
            await _context.SaveChangesAsync();

            // Přidat tvůrce jako admina týmu
            var membership = new TeamMember
            {
                UserId = creatorUserId,
                TeamId = team.Id,
                Role = TeamRole.Admin
            };

            _context.TeamMembers.Add(membership);
            await _context.SaveChangesAsync();

            return team;

        }

        public async Task<TeamStatistics> GetTeamStatisticsAsync(int teamId, int season)
        {
            return await _context.TeamStatistics
                .FirstOrDefaultAsync(ts => ts.TeamId == teamId && ts.Season == season);
        }

        public async Task<bool> InviteUserToTeamAsync(int teamId, string userId, TeamRole role)
        {
            // Kontrola, zda uživatel již není členem
            var existingMember = await _context.TeamMembers
                .FirstOrDefaultAsync(tm => tm.TeamId == teamId && tm.UserId == userId && tm.IsActive);

            if (existingMember != null)
                return false;

            var team = await _context.Teams.FindAsync(teamId);
            if (team == null)
                return false;

            // Odeslat notifikaci o pozvánce
            await _notificationService.SendNotificationAsync(
                userId,
                NotificationType.TeamInvitation,
                "Pozvánka do týmu",
                $"Byli jste pozváni do týmu {team.Name}",
                teamId
            );

            return true;

        }

        public async Task<bool> RemoveUserFromTeamAsync(int teamId, string userId)
        {
            var membership = await _context.TeamMembers
                .FirstOrDefaultAsync(tm => tm.TeamId == teamId && tm.UserId == userId && tm.IsActive);

            if (membership == null)
                return false;

            membership.IsActive = false;
            membership.LeftAt = DateTime.Now;
            var result = await _context.SaveChangesAsync();

            return result > 0;

        }

        public async Task<bool> UpdateTeamRoleAsync(int teamId, string userId, TeamRole newRole)
        {
            var membership = await _context.TeamMembers
                .FirstOrDefaultAsync(tm => tm.TeamId == teamId && tm.UserId == userId && tm.IsActive);

            if (membership == null)
                return false;

            membership.Role = newRole;
            var result = await _context.SaveChangesAsync();

            return result > 0;

        }
    }
}
