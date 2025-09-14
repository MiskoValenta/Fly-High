using FlyHigh.DTOs.PollDTOs;
using FlyHigh.Models.TeamModels;

namespace FlyHigh.Services.Interfaces
{
    public interface IPollService
    {
        Task<Poll> CreatePollAsync(CreatePollDto pollDto, string creatorUserId);
        Task<bool> VotePollAsync(int pollId, int optionId, string userId, string comment = null);
        Task<PollResultDto> GetPollResultsAsync(int pollId);
        Task<IEnumerable<Poll>> GetTeamPollsAsync(int teamId, bool activeOnly = true);
        Task<bool> ClosePollAsync(int pollId, string userId);

    }
}
