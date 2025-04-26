using Application.DtoModels.KidQuiz.Lessons;
using Application.Services.Interfaces.IRepository.KidQuiz;
using AutoMapper;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class KidLessonRepository : IKidLessonRepository
{
    private readonly LanguageLearningDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<KidLessonRepository> _logger;

    public KidLessonRepository(LanguageLearningDbContext context, IMapper mapper, ILogger<KidLessonRepository> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<List<KidLessonDto>> GetAllLessonsAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving all KidLessons with WordCards...");

            var lessons = await _context.KidLessons
                .Include(x => x.WordCards)
                .ToListAsync();

            _logger.LogInformation("Successfully retrieved {Count} KidLessons", lessons.Count);

            return _mapper.Map<List<KidLessonDto>>(lessons);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve all KidLessons");
            return new List<KidLessonDto>();
        }
    }

    public async Task<KidLesson?> GetLessonByIdAsync(int id)
    {
        try
        {
            _logger.LogInformation("Retrieving KidLesson with ID {Id}", id);

            var lesson = await _context.KidLessons
                .Include(x => x.WordCards)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (lesson == null)
            {
                _logger.LogWarning("KidLesson with ID {Id} not found", id);
                return null;
            }

            _logger.LogInformation("Successfully retrieved KidLesson with ID {Id}", id);
            
            return _mapper.Map<KidLesson>(lesson);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve KidLesson with ID {Id}", id);
            return null;
        }
    }

    public async Task<KidLessonDto?> CreateLessonAsync(KidLesson lesson)
    {
        try
        {
            _logger.LogInformation("Creating new KidLesson...");

            _context.KidLessons.Add(lesson);
            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                _logger.LogInformation("Successfully created KidLesson with ID {Id}", lesson.Id);
                return _mapper.Map<KidLessonDto>(lesson);
            }
            else
            {
                _logger.LogWarning("No changes were made while creating KidLesson");
                return null;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create KidLesson");
            return null;
        }
    }

    public async Task<KidLesson?> UpdateLessonAsync(int id, KidLesson lesson)
    {
        try
        {
            _logger.LogInformation("Updating KidLesson with ID {Id}", id);

            var existingLesson = await _context.KidLessons.FindAsync(id);
            if (existingLesson == null)
            {
                _logger.LogWarning("KidLesson with ID {Id} not found", id);
                return null;
            }

            existingLesson.Title = lesson.Title ?? existingLesson.Title;
            existingLesson.Description = lesson.Description ?? existingLesson.Description;
            existingLesson.ImageUrl = lesson.ImageUrl ?? existingLesson.ImageUrl;

            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                _logger.LogInformation("Successfully updated KidLesson with ID {Id}", id);
            }
            else
            {
                _logger.LogWarning("No changes were made while updating KidLesson with ID {Id}", id);
            }

            return existingLesson;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update KidLesson with ID {Id}", id);
            return null;
        }
    }

    public async Task<KidLessonDto?> DeleteLessonAsync(int id)
    {
        try
        {
            _logger.LogInformation("Deleting KidLesson with ID {Id}", id);

            var lesson = await _context.KidLessons.FindAsync(id);
            if (lesson == null)
            {
                _logger.LogWarning("KidLesson with ID {Id} not found", id);
                return null;
            }

            _context.KidLessons.Remove(lesson);
            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                _logger.LogInformation("Successfully deleted KidLesson with ID {Id}", id);
                return _mapper.Map<KidLessonDto>(lesson);
            }
            else
            {
                _logger.LogWarning("No changes were made while deleting KidLesson with ID {Id}", id);
                return null;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete KidLesson with ID {Id}", id);
            return null;
        }
    }
}