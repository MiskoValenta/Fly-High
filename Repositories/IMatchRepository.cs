using System.Text.RegularExpressions;
using DbMatch = FlyHigh.Models.MatchModels.Match;

namespace FlyHigh.Repositories
{
    public interface IMatchRepository
    {
        Task<IEnumerable<DbMatch>> GetAllAsync();
        Task<DbMatch?> GetByIdAsync(int id);
        Task<IEnumerable<DbMatch>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<DbMatch>> SearchByTeamAsync(string teamName);
        Task<DbMatch> CreateAsync(DbMatch match);
        Task UpdateAsync(DbMatch match);
        Task DeleteAsync(int id);
    }
}
