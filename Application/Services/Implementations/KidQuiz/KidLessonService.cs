using Application.DtoModels.KidQuiz.Lessons;
using Application.Services.Interfaces.IServices.KidQuiz;
using Application.UnitOfWork;
using AutoMapper;
using Domain.Models;
using Microsoft.Extensions.Logging;

namespace Application.Services.Implementations.KidQuiz
{
    public class KidLessonService : IKidLessonService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<KidLessonService> _logger;

        public KidLessonService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<KidLessonService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<KidLessonDto>> GetAllLessonsAsync()
        {
            try
            {
                var lessons = await _unitOfWork.KidLessonRepository.GetAllLessonsAsync();
                _logger.LogInformation("Successfully retrieved {Count} KidLessons", lessons.Count);
                
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
                var lesson = await _unitOfWork.KidLessonRepository.GetLessonByIdAsync(id);
                if (lesson == null)
                {
                    _logger.LogWarning("KidLesson with ID {Id} not found in service layer", id);
                    return null;
                }

                _logger.LogInformation("Successfully retrieved KidLesson with ID {Id}", id);
               
                return _mapper.Map<KidLessonDto>(lesson);
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
                var createdLesson = await _unitOfWork.KidLessonRepository.CreateLessonAsync(lesson);

                if (createdLesson == null)
                {
                    _logger.LogWarning("Failed to create KidLesson - repository returned null");
                    return null;
                }

                _logger.LogInformation("Successfully created KidLesson with ID {Id}", createdLesson.Id);
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
                var existingLesson = await _unitOfWork.KidLessonRepository.GetLessonByIdAsync(id);
                if (existingLesson == null)
                {
                    _logger.LogWarning("KidLesson with ID {Id} not found for update", id);
                    return null;
                }

                existingLesson.Title = !string.IsNullOrEmpty(dto.Title) ? dto.Title : existingLesson.Title;
                existingLesson.Description = !string.IsNullOrEmpty(dto.Description) ? dto.Description : existingLesson.Description;
                existingLesson.ImageUrl = !string.IsNullOrEmpty(dto.ImageUrl) ? dto.ImageUrl : existingLesson.ImageUrl;

                var updatedLesson = await _unitOfWork.KidLessonRepository.UpdateLessonAsync(id, existingLesson);
                if (updatedLesson == null)
                {
                    _logger.LogWarning("Failed to update KidLesson with ID {Id} - repository returned null", id);
                    return null;
                }

                _logger.LogInformation("Successfully updated KidLesson with ID {Id}", id);
                
                return _mapper.Map<KidLessonDto>(updatedLesson);
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
                var deletedLesson = await _unitOfWork.KidLessonRepository.DeleteLessonAsync(id);
                if (deletedLesson == null)
                {
                    _logger.LogWarning("KidLesson with ID {Id} not found for deletion", id);
                    return null;
                }

                _logger.LogInformation("Successfully deleted KidLesson with ID {Id}", id);
                
                return deletedLesson;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete KidLesson with ID {Id} in the service layer", id);
                return null;
            }
        }
    }
}