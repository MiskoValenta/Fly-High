using FlyHigh.Data;
using FlyHigh.DTOs.TrainingDto;
using FlyHigh.DTOs.UserDTOs;
using FlyHigh.Models;
using FlyHigh.Models.TeamModels;
using FlyHigh.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlyHigh.Services
{
    public class TrainingService : ITrainingService
    {
        private readonly VolleyballDbContext _context;
        private readonly INotificationService _notificationService;

        public TrainingService(VolleyballDbContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        public async Task<bool> CancelTrainingAsync(int trainingId, string reason, string userId)
        {
            var training = await _context.Trainings.FindAsync(trainingId);
            if (training == null)
                return false;

            training.IsCancelled = true;
            training.CancellationReason = reason;

            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                // Poslat notifikaci o zrušení
                await _notificationService.SendTeamNotificationAsync(
                    training.TeamId,
                    NotificationType.TrainingCancelled,
                    "Trénink zrušen",
                    $"Trénink {training.Title} na {training.ScheduledDate:dd.MM.yyyy} byl zrušen. Důvod: {reason}",
                    training.Id
                );
            }

            return result > 0;
        }

        public async Task<Training> CreateTrainingAsync(CreateTrainingDto trainingDto, string creatorUserId)
        {
            var training = new Training
            {
                TeamId = trainingDto.TeamId,
                Title = trainingDto.Title,
                Description = trainingDto.Description,
                ScheduledDate = trainingDto.ScheduledDate,
                StartTime = trainingDto.StartTime,
                EndTime = trainingDto.EndTime,
                Location = trainingDto.Location,
                LocationAddress = trainingDto.LocationAddress
            };

            _context.Trainings.Add(training);
            await _context.SaveChangesAsync();

            // Poslat notifikaci všem členům týmu
            await _notificationService.SendTeamNotificationAsync(
                training.TeamId,
                NotificationType.TrainingScheduled,
                "Naplánován trénink",
                $"Byl naplánován trénink: {training.Title} na {training.ScheduledDate:dd.MM.yyyy} v {training.StartTime}",
                training.Id
            );

            return training;
        }

        public async Task<IEnumerable<Training>> GetTeamTrainingsAsync(int teamId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.Trainings
                .Include(t => t.Attendances)
                    .ThenInclude(a => a.User)
                .Where(t => t.TeamId == teamId);

            if (startDate.HasValue)
                query = query.Where(t => t.ScheduledDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(t => t.ScheduledDate <= endDate.Value);

            return await query
                .OrderBy(t => t.ScheduledDate)
                .ThenBy(t => t.StartTime)
                .ToListAsync();

        }

        public async Task<TrainingAttendanceDto> GetTrainingAttendanceAsync(int trainingId)
        {
            var training = await _context.Trainings
                .Include(t => t.Attendances)
                    .ThenInclude(a => a.User)
                .FirstOrDefaultAsync(t => t.Id == trainingId);

            if (training == null)
                return null;

            var attendances = training.Attendances.Select(a => new TrainingAttendanceDto
            {
                Id = a.Id,
                TrainingId = a.TrainingId,
                UserId = a.UserId,
                Status = a.Status.ToString(),
                Note = a.Note,
                ResponseAt = a.ResponseAt,
                User = new UserSummaryDto
                {
                    Id = a.User.Id,
                    FirstName = a.User.FirstName,
                    LastName = a.User.LastName,
                    FullName = a.User.FullName,
                    Email = a.User.Email
                }
            }).ToList();

            return new TrainingAttendanceDto
            {
                Id = training.Id,
                TrainingId = training.Id,
                // Možná by mělo být jiné DTO pro summary
                // ale použiju to co mám
            };

        }

        public async Task<bool> RespondToTrainingAsync(int trainingId, string userId, AttendanceStatus status, string note = null)
        {
            // Kontrola, zda trénink existuje
            var training = await _context.Trainings.FindAsync(trainingId);
            if (training == null || training.IsCancelled)
                return false;

            // Najít nebo vytvořit attendance záznam
            var attendance = await _context.TrainingAttendances
                .FirstOrDefaultAsync(ta => ta.TrainingId == trainingId && ta.UserId == userId);

            if (attendance == null)
            {
                attendance = new TrainingAttendance
                {
                    TrainingId = trainingId,
                    UserId = userId,
                    Status = status,
                    Note = note
                };
                _context.TrainingAttendances.Add(attendance);
            }
            else
            {
                attendance.Status = status;
                attendance.Note = note;
                attendance.UpdatedAt = DateTime.Now;
            }

            var result = await _context.SaveChangesAsync();
            return result > 0;

        }
    }
}
