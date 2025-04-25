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
            var lessons = await _context.KidLessons.ToListAsync();
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
            var lesson = await _context.KidLessons.FindAsync(id);
            if (lesson == null)
            {
                _logger.LogWarning("KidLesson with ID {Id} not found", id);
                return null;
            }

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
            _context.KidLessons.Add(lesson);
            await _context.SaveChangesAsync();

            return _mapper.Map<KidLessonDto>(lesson);
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
            var existingLesson = await _context.KidLessons.FindAsync(id);
            if (existingLesson == null)
            {
                _logger.LogWarning("KidLesson with ID {Id} not found", id);
                return null;
            }

            existingLesson.Title = lesson.Title ?? existingLesson.Title;
            existingLesson.Description = lesson.Description ?? existingLesson.Description;
            existingLesson.ImageUrl = lesson.ImageUrl ?? existingLesson.ImageUrl;

            await _context.SaveChangesAsync();

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
            var lesson = await _context.KidLessons.FindAsync(id);
            if (lesson == null)
            {
                _logger.LogWarning("KidLesson with ID {Id} not found", id);
                return null;
            }

            _context.KidLessons.Remove(lesson);
            await _context.SaveChangesAsync();

            return _mapper.Map<KidLessonDto>(lesson);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete KidLesson with ID {Id}", id);
            return null;
        }
    }
}
