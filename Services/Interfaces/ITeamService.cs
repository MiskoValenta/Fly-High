using FlyHigh.DTOs.TeamDTOs;
using FlyHigh.Models.MatchModels;
using FlyHigh.Models.TeamModels;

namespace FlyHigh.Services.Interfaces
{
    public interface ITeamService
    {
        Task<Team> CreateTeamAsync(CreateTeamDto teamDto, string creatorUserId);
        Task<bool> InviteUserToTeamAsync(int teamId, string userId, TeamRole role);
        Task<bool> AcceptTeamInvitationAsync(int teamId, string userId);
        Task<IEnumerable<Team>> GetUserTeamsAsync(string userId);
        Task<TeamStatistics> GetTeamStatisticsAsync(int teamId, int season);
        Task<bool> UpdateTeamRoleAsync(int teamId, string userId, TeamRole newRole);
        Task<bool> RemoveUserFromTeamAsync(int teamId, string userId);

    }
}
