using FlyHigh.Data;
using FlyHigh.DTOs.PollDTOs;
using FlyHigh.DTOs.UserDTOs;
using FlyHigh.Models;
using FlyHigh.Models.TeamModels;
using FlyHigh.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlyHigh.Services
{
    public class PollService : IPollService
    {
        private readonly VolleyballDbContext _context;
        private readonly INotificationService _notificationService;

        public PollService(VolleyballDbContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }
        public async Task<bool> ClosePollAsync(int pollId, string userId)
        {
            var poll = await _context.Polls
                .FirstOrDefaultAsync(p => p.Id == pollId && p.CreatedByUserId == userId);

            if (poll == null)
                return false;

            poll.IsActive = false;
            var result = await _context.SaveChangesAsync();

            return result > 0;

        }

        public async Task<Poll> CreatePollAsync(CreatePollDto pollDto, string creatorUserId)
        {
            var poll = new Poll
            {
                TeamId = pollDto.TeamId,
                CreatedByUserId = creatorUserId,
                Title = pollDto.Title,
                Description = pollDto.Description,
                Type = Enum.Parse<PollType>(pollDto.Type),
                IsMultipleChoice = pollDto.IsMultipleChoice,
                IsAnonymous = pollDto.IsAnonymous,
                ExpiresAt = pollDto.ExpiresAt
            };

            _context.Polls.Add(poll);
            await _context.SaveChangesAsync();

            // Přidat možnosti
            foreach (var optionDto in pollDto.Options)
            {
                var option = new PollOption
                {
                    PollId = poll.Id,
                    Text = optionDto.Text,
                    Order = optionDto.Order
                };
                _context.PollOptions.Add(option);
            }

            await _context.SaveChangesAsync();

            // Poslat notifikaci všem členům týmu
            await _notificationService.SendTeamNotificationAsync(
                poll.TeamId,
                NotificationType.PollCreated,
                "Nová anketa",
                $"Byla vytvořena nová anketa: {poll.Title}",
                poll.Id
            );

            return poll;

        }

        public async Task<PollResultDto> GetPollResultsAsync(int pollId)
        {
            var poll = await _context.Polls
                .Include(p => p.Options)
                    .ThenInclude(o => o.Responses)
                        .ThenInclude(r => r.User)
                .Include(p => p.CreatedByUser)
                .Include(p => p.Responses)
                    .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(p => p.Id == pollId);

            if (poll == null)
                return null;

            var totalVotes = poll.Responses.Count;

            var results = poll.Options.Select(option => new PollOptionResultDto
            {
                Option = new PollOptionDto
                {
                    Id = option.Id,
                    PollId = option.PollId,
                    Text = option.Text,
                    Order = option.Order,
                    ResponseCount = option.Responses.Count,
                    Percentage = totalVotes > 0 ? (double)option.Responses.Count / totalVotes * 100 : 0
                },
                VoteCount = option.Responses.Count,
                Percentage = totalVotes > 0 ? (double)option.Responses.Count / totalVotes * 100 : 0,
                Voters = poll.IsAnonymous ? new List<UserSummaryDto>() :
                    option.Responses.Select(r => new UserSummaryDto
                    {
                        Id = r.User.Id,
                        FirstName = r.User.FirstName,
                        LastName = r.User.LastName,
                        FullName = r.User.FullName,
                        Email = r.User.Email
                    }).ToList()
            }).ToList();

            var comments = poll.Responses
                .Where(r => !string.IsNullOrEmpty(r.Comment))
                .Select(r => new PollResponseDto
                {
                    Id = r.Id,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt,
                    User = poll.IsAnonymous ? null : new UserSummaryDto
                    {
                        Id = r.User.Id,
                        FirstName = r.User.FirstName,
                        LastName = r.User.LastName,
                        FullName = r.User.FullName,
                        Email = r.User.Email
                    }
                }).ToList();

            return new PollResultDto
            {
                Poll = new PollDetailDto
                {
                    Id = poll.Id,
                    TeamId = poll.TeamId,
                    CreatedByUserId = poll.CreatedByUserId,
                    Title = poll.Title,
                    Description = poll.Description,
                    Type = poll.Type.ToString(),
                    IsMultipleChoice = poll.IsMultipleChoice,
                    IsAnonymous = poll.IsAnonymous,
                    ExpiresAt = poll.ExpiresAt,
                    IsActive = poll.IsActive,
                    CreatedAt = poll.CreatedAt,
                    CreatedByUser = new UserSummaryDto
                    {
                        Id = poll.CreatedByUser.Id,
                        FirstName = poll.CreatedByUser.FirstName,
                        LastName = poll.CreatedByUser.LastName,
                        FullName = poll.CreatedByUser.FullName,
                        Email = poll.CreatedByUser.Email
                    },
                    Options = poll.Options.Select(o => new PollOptionDto
                    {
                        Id = o.Id,
                        PollId = o.PollId,
                        Text = o.Text,
                        Order = o.Order,
                        ResponseCount = o.Responses.Count,
                        Percentage = totalVotes > 0 ? (double)o.Responses.Count / totalVotes * 100 : 0
                    }).ToList()
                },
                Results = results,
                TotalVotes = totalVotes,
                Comments = comments
            };

        }

        public async Task<IEnumerable<Poll>> GetTeamPollsAsync(int teamId, bool activeOnly = true)
        {
            var query = _context.Polls
                .Include(p => p.CreatedByUser)
                .Include(p => p.Options)
                .Where(p => p.TeamId == teamId);

            if (activeOnly)
            {
                query = query.Where(p => p.IsActive && (!p.ExpiresAt.HasValue || p.ExpiresAt > DateTime.Now));
            }

            return await query.OrderByDescending(p => p.CreatedAt).ToListAsync();
        }

        public async Task<bool> VotePollAsync(int pollId, int optionId, string userId, string comment = null)
        {
            // Kontrola, zda poll existuje a je aktivní
            var poll = await _context.Polls
                .Include(p => p.Options)
                .FirstOrDefaultAsync(p => p.Id == pollId && p.IsActive);

            if (poll == null || (poll.ExpiresAt.HasValue && poll.ExpiresAt < DateTime.Now))
                return false;

            // Kontrola, zda možnost existuje
            var option = poll.Options.FirstOrDefault(o => o.Id == optionId);
            if (option == null)
                return false;

            // Kontrola, zda uživatel již nehlasoval (pro single choice)
            if (!poll.IsMultipleChoice)
            {
                var existingVote = await _context.PollResponses
                    .AnyAsync(pr => pr.PollId == pollId && pr.UserId == userId);

                if (existingVote)
                    return false;
            }

            var response = new PollResponse
            {
                PollId = pollId,
                PollOptionId = optionId,
                UserId = userId,
                Comment = comment
            };

            _context.PollResponses.Add(response);
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }

    }
}

