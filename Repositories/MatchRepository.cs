using FlyHigh.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using DbMatch = FlyHigh.Models.MatchModels.Match;

namespace FlyHigh.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        private readonly VolleyballDbContext _context;

        public MatchRepository(VolleyballDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DbMatch>> GetAllAsync()
        {
            return await _context.Matches
                .Include(m => m.Sets)
                .Include(m => m.Penalties)
                .Include(m => m.MatchResult)
                .Include(m => m.HomeTeamEntity)
                .Include(m => m.AwayTeamEntity)
                .OrderByDescending(m => m.MatchDate)
                .ToListAsync();
        }

        public async Task<DbMatch?> GetByIdAsync(int id)
        {
            return await _context.Matches
                .Include(m => m.Sets)
                    .ThenInclude(s => s.Points)
                .Include(m => m.Penalties)
                .Include(m => m.MatchResult)
                .Include(m => m.HomeTeamEntity)
                .Include(m => m.AwayTeamEntity)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<DbMatch>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Matches
                .Include(m => m.MatchResult)
                .Where(m => m.MatchDate >= startDate && m.MatchDate <= endDate)
                .OrderBy(m => m.MatchDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<DbMatch>> SearchByTeamAsync(string teamName)
        {
            return await _context.Matches
                .Include(m => m.MatchResult)
                .Where(m => m.HomeTeam.Contains(teamName) || m.AwayTeam.Contains(teamName))
                .OrderByDescending(m => m.MatchDate)
                .ToListAsync();
        }

        public async Task<DbMatch> CreateAsync(DbMatch match)
        {
            _context.Matches.Add(match);
            await _context.SaveChangesAsync();
            return match;
        }

        public async Task UpdateAsync(DbMatch match)
        {
            match.UpdatedAt = DateTime.Now;
            _context.Entry(match).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var match = await _context.Matches.FindAsync(id);
            if (match != null)
            {
                _context.Matches.Remove(match);
                await _context.SaveChangesAsync();
            }
        }
    }

}
