using Application.DtoModels.Lessons.Lessons;
using Application.DtoModels.Lessons.Quiz;
using Application.Services.Interfaces.IServices;
using Application.UnitOfWork;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Application.Services.Implementations.Lesson
{
    public class LessonService : ILessonService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<LessonService> _logger;

        public LessonService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<LessonService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<LessonDto> GetLessonByIdAsync(int id)
        {

            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid lesson ID requested: {Id}", id);
                    throw new ArgumentException("Lesson ID must be a positive integer", nameof(id));
                }

                var lesson = await _unitOfWork.LessonRepository.GetLessonByIdAsync(id);

                _logger.LogInformation("Retrieved lesson with {QuizCount} quizzes", lesson.Quizzes.Count);

                return _mapper.Map<LessonDto>(lesson);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Lesson with ID {Id} not found", id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while getting lesson with ID {Id}", id);
                throw new ApplicationException($"An error occurred while retrieving lesson {id}", ex);
            }
        }

        public async Task<List<LessonDto>> GetAllLessonsAsync()
        {
            try
            {
                var lessons = await _unitOfWork.LessonRepository.GetAllLessonsAsync();
                
                return _mapper.Map<List<LessonDto>>(lessons);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "No lessons found in the system");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while retrieving all lessons");
                throw new ApplicationException("An error occurred while retrieving all lessons", ex);
            }
        }

        public async Task<LessonDto> CreateLessonAsync(CreateLessonDto createDto)
        {

            try
            {
                if (createDto == null)
                {
                    _logger.LogWarning("Attempt to create lesson with null DTO");
                    throw new ArgumentNullException(nameof(createDto), "Lesson creation data cannot be null");
                }

                var lesson = _mapper.Map<Domain.Models.Lesson>(createDto);
                var createdLesson = await _unitOfWork.LessonRepository.AddLessonAsync(lesson);

                _logger.LogInformation("Successfully created new lesson with ID {Id}", createdLesson.Id);
                return _mapper.Map<LessonDto>(createdLesson);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating new lesson");
                throw new ApplicationException("An error occurred while creating the lesson", ex);
            }
        }

        public async Task<LessonDto> UpdateLessonAsync(UpdateLessonDto updateDto)
        {

            try
            {
                if (updateDto == null)
                {
                    _logger.LogWarning("Attempt to update lesson with null DTO");
                    throw new ArgumentNullException(nameof(updateDto), "Lesson update data cannot be null");
                }

                if (updateDto.Id <= 0)
                {
                    _logger.LogWarning("Attempt to update lesson with invalid ID: {Id}", updateDto.Id);
                    throw new ArgumentException("Lesson ID must be a positive integer", nameof(updateDto.Id));
                }

                var updatedLesson = await _unitOfWork.LessonRepository.UpdateLessonAsync(
                    updateDto.Id,
                    lesson =>
                    {
                        if (!string.IsNullOrWhiteSpace(updateDto.Title))
                            lesson.Title = updateDto.Title;
                        if (!string.IsNullOrWhiteSpace(updateDto.Description))
                            lesson.Description = updateDto.Description;
                        if (!string.IsNullOrWhiteSpace(updateDto.VideoUrl))
                            lesson.VideoUrl = updateDto.VideoUrl;
                        if (!string.IsNullOrWhiteSpace(updateDto.PdfUrl))
                            lesson.PdfUrl = updateDto.PdfUrl;
                    });

                _logger.LogInformation("Successfully updated lesson with ID {Id}", updateDto.Id);
                return _mapper.Map<LessonDto>(updatedLesson);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Lesson with ID {Id} not found for update", updateDto.Id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating lesson with ID {Id}", updateDto.Id);
                throw new ApplicationException($"An error occurred while updating lesson {updateDto.Id}", ex);
            }
        }

        public async Task<LessonDto?> DeleteLessonAsync(int id)
        {

            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Attempt to delete lesson with invalid ID: {Id}", id);
                    throw new ArgumentException("Lesson ID must be a positive integer", nameof(id));
                }

                var deletedLesson = await _unitOfWork.LessonRepository.DeleteLessonAsync(id);

                if (deletedLesson == null)
                {
                    _logger.LogWarning("Lesson with ID {Id} not found or already deleted", id);
                    return null;
                }

                return _mapper.Map<LessonDto>(deletedLesson);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Lesson with ID {Id} not found for deletion", id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting lesson with ID {Id}", id);
                throw new ApplicationException($"An error occurred while deleting lesson {id}", ex);
            }
        }

        public async Task<List<LessonWordDto>> GetLessonWordsAsync(int lessonId)
        {

            try
            {
                if (lessonId <= 0)
                {
                    _logger.LogWarning("Attempt to get words with invalid lesson ID: {LessonId}", lessonId);
                    throw new ArgumentException("Lesson ID must be a positive integer", nameof(lessonId));
                }

                var words = await _unitOfWork.LessonRepository.GetWordsByLessonAsync(lessonId);
                
                return _mapper.Map<List<LessonWordDto>>(words);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "No words found for lesson ID {LessonId}", lessonId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving words for lesson ID {LessonId}", lessonId);
                throw new ApplicationException($"An error occurred while retrieving words for lesson {lessonId}", ex);
            }
        }

        public async Task<List<LessonPhraseDto>> GetLessonPhrasesAsync(int lessonId)
        {

            try
            {
                if (lessonId <= 0)
                {
                    _logger.LogWarning("Attempt to get phrases with invalid lesson ID: {LessonId}", lessonId);
                    throw new ArgumentException("Lesson ID must be a positive integer", nameof(lessonId));
                }

                var phrases = await _unitOfWork.LessonRepository.GetPhrasesByLessonAsync(lessonId);
                
                return _mapper.Map<List<LessonPhraseDto>>(phrases);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "No phrases found for lesson ID {LessonId}", lessonId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving phrases for lesson ID {LessonId}", lessonId);
                throw new ApplicationException($"An error occurred while retrieving phrases for lesson {lessonId}", ex);
            }
        }

        public async Task<List<QuizDto>> GetLessonQuizzesAsync(int lessonId)
        {

            try
            {
                if (lessonId <= 0)
                {
                    _logger.LogWarning("Attempt to get quizzes with invalid lesson ID: {LessonId}", lessonId);
                    throw new ArgumentException("Lesson ID must be a positive integer", nameof(lessonId));
                }

                var quizzes = await _unitOfWork.LessonRepository.GetQuizzesByLessonAsync(lessonId);
                
                return _mapper.Map<List<QuizDto>>(quizzes);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "No quizzes found for lesson ID {LessonId}", lessonId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving quizzes for lesson ID {LessonId}", lessonId);
                throw new ApplicationException($"An error occurred while retrieving quizzes for lesson {lessonId}", ex);
            }
        }

        public async Task<LessonDetailDto> GetLessonDetailAsync(int id)
        {

            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Attempt to get lesson detail with invalid ID: {Id}", id);
                    throw new ArgumentException("Lesson ID must be a positive integer", nameof(id));
                }

                var lesson = await _unitOfWork.LessonRepository.GetLessonByIdAsync(id);
                var dto = _mapper.Map<LessonDetailDto>(lesson);

                dto.TotalWordsCount = lesson.Words?.Count ?? 0;
                dto.CompletedWordsCount = await CalculateCompletedWords(id);

                return dto;
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Lesson with ID {Id} not found for detail view", id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving detail for lesson ID {Id}", id);
                throw new ApplicationException($"An error occurred while retrieving detail for lesson {id}", ex);
            }
        }

        private async Task<int> CalculateCompletedWords(int lessonId)
        {
            return 0;
        }
    }
}