using Application.DtoModels.Lessons.Quiz;
using Application.Services.Interfaces.IServices.Lesons;
using Application.UnitOfWork;
using AutoMapper;
using Common.Exceptions;
using Domain.Models;
using LangLearningAPI.Exceptions;
using Microsoft.Extensions.Logging;

namespace Application.Services.Implementations.Lesson.IQuizServ
{
    public class LessonQuizService : IQuizService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<LessonQuizService> _logger;

        public LessonQuizService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<LessonQuizService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<QuizDto> CreateQuizAsync(CreateQuizDto dto)
        {
            try
            {
                _logger.LogInformation("Creating new quiz for lesson {LessonId}", dto.LessonId);

                if (string.IsNullOrWhiteSpace(dto.Type))
                    throw new ServiceException("Quiz type is required", "VALIDATION_ERROR");

                var lessonExists = await _unitOfWork.LessonRepository.GetAllLessonsAsync();
                if (!lessonExists.Any(l => l.Id == dto.LessonId))
                    throw new NotFoundException($"Lesson with ID {dto.LessonId} not found", "LESSON_NOT_FOUND");

                var quizExists = await _unitOfWork.QuizRepository.QuizExistsInLessonAsync(dto.LessonId, dto.Type);
                if (quizExists)
                    throw new ServiceException($"Quiz of type {dto.Type} already exists in this lesson", "DUPLICATE_QUIZ");

                var quiz = _mapper.Map<Quiz>(dto);
                var createdQuiz = await _unitOfWork.QuizRepository.CreateQuizAsync(quiz);

                return _mapper.Map<QuizDto>(createdQuiz);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating quiz");
                throw;
            }
        }

        public async Task<QuizDto> DeleteQuizAsync(int id)
        {
            try
            {
                _logger.LogInformation("Deleting quiz with ID: {QuizId}", id);

                var deletedQuiz = await _unitOfWork.QuizRepository.DeleteQuizAsync(id);
                
                return _mapper.Map<QuizDto>(deletedQuiz);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting quiz with ID: {QuizId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<QuizDto>> GetAllQuizzesAsync()
        {
            try
            {
                var quizzes = await _unitOfWork.QuizRepository.GetAllQuizzesAsync();
                
                return _mapper.Map<IEnumerable<QuizDto>>(quizzes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all quizzes");
                throw;
            }
        }

        public async Task<QuizDto> GetQuizByIdAsync(int id)
        {
            try
            {
                var quiz = await _unitOfWork.QuizRepository.GetQuizByIdAsync(id);
                
                return _mapper.Map<QuizDto>(quiz);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting quiz by ID: {QuizId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<QuizDto>> GetQuizzesByLessonIdAsync(int lessonId)
        {
            try
            {
                var quizzes = await _unitOfWork.QuizRepository.GetQuizzesByLessonIdAsync(lessonId);
                
                return _mapper.Map<IEnumerable<QuizDto>>(quizzes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting quizzes for lesson {LessonId}", lessonId);
                throw;
            }
        }

        public async Task<QuizDto> UpdateQuizAsync(UpdateQuizDto dto)
        {
            try
            {
                _logger.LogInformation("Updating quiz with ID: {QuizId}", dto.Id);

                var existingQuiz = await _unitOfWork.QuizRepository.GetQuizByIdAsync(dto.Id);
                if (existingQuiz == null)
                    throw new NotFoundException($"Quiz with ID {dto.Id} not found", "QUIZ_NOT_FOUND");

                if (!string.IsNullOrEmpty(dto.Type))
                {
                    existingQuiz.Type = dto.Type;
                }

                var updatedQuiz = await _unitOfWork.QuizRepository.UpdateQuizAsync(existingQuiz);
                
                return _mapper.Map<QuizDto>(updatedQuiz);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating quiz with ID: {QuizId}", dto.Id);
                throw;
            }
        }
    }
}
