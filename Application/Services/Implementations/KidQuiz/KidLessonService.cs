using Application.DtoModels.KidQuiz.Lessons;
using Application.Services.Interfaces.IRepository.KidQuiz;
using Application.Services.Interfaces.IServices.KidQuiz;
using AutoMapper;
using Domain.Models;
using Microsoft.Extensions.Logging;

namespace Application.Services.Implementations.KidQuiz
{
    public class KidLessonService : IKidLessonService
    {
        private readonly IKidLessonRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<KidLessonService> _logger;

        public KidLessonService(IKidLessonRepository repository, IMapper mapper, ILogger<KidLessonService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<KidLessonDto>> GetAllLessonsAsync()
        {
            try
            {
                var lessons = await _repository.GetAllLessonsAsync();
                return _mapper.Map<List<KidLessonDto>>(lessons);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve all KidLessons from the service layer");
                return new List<KidLessonDto>();
            }
        }

        public async Task<KidLessonDto?> GetLessonByIdAsync(int id)
        {
            try
            {
                var lesson = await _repository.GetLessonByIdAsync(id);
                return lesson == null ? null : _mapper.Map<KidLessonDto>(lesson);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve KidLesson with ID {Id} from the service layer", id);
                return null;
            }
        }

        public async Task<KidLessonDto?> CreateLessonAsync(CreateKidLessonDto dto)
        {
            try
            {
                var lesson = _mapper.Map<KidLesson>(dto);
                var createdLesson = await _repository.CreateLessonAsync(lesson);

                return _mapper.Map<KidLessonDto>(createdLesson);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create KidLesson in the service layer");
                return null;
            }
        }

        public async Task<KidLessonDto?> UpdateLessonAsync(int id, UpdateKidLessonDto dto)
        {
            try
            {
                var existingLesson = await _repository.GetLessonByIdAsync(id);
                if (existingLesson == null) return null;

                existingLesson.Title = !string.IsNullOrEmpty(dto.Title) ? dto.Title : existingLesson.Title;
                existingLesson.Description = !string.IsNullOrEmpty(dto.Description) ? dto.Description : existingLesson.Description;
                existingLesson.ImageUrl = !string.IsNullOrEmpty(dto.ImageUrl) ? dto.ImageUrl : existingLesson.ImageUrl;

                var updatedLesson = await _repository.UpdateLessonAsync(id, existingLesson);
                
                return updatedLesson == null ? null : _mapper.Map<KidLessonDto>(updatedLesson);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update KidLesson with ID {Id} in the service layer", id);
                return null;
            }
        }

        public async Task<KidLessonDto?> DeleteLessonAsync(int id)
        {
            try
            {
                var lesson = await _repository.DeleteLessonAsync(id);
                return lesson == null ? null : _mapper.Map<KidLessonDto>(lesson);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete KidLesson with ID {Id} in the service layer", id);
                return null;
            }
        }
    }
}
