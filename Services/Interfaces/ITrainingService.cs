using FlyHigh.DTOs.TrainingDto;
using FlyHigh.Models.TeamModels;

namespace FlyHigh.Services.Interfaces
{
    public interface ITrainingService
    {
        Task<Training> CreateTrainingAsync(CreateTrainingDto trainingDto, string creatorUserId);
        Task<bool> RespondToTrainingAsync(int trainingId, string userId, AttendanceStatus status, string note = null);
        Task<TrainingAttendanceDto> GetTrainingAttendanceAsync(int trainingId);
        Task<IEnumerable<Training>> GetTeamTrainingsAsync(int teamId, DateTime? startDate = null, DateTime? endDate = null);
        Task<bool> CancelTrainingAsync(int trainingId, string reason, string userId);

    }
}
