using AutoMapper;
using Common.Exceptions;
using Domain.Models;
using LangLearningAPI.Exceptions;
using Microsoft.Extensions.Logging;
using Application.DtoModels.Lessons.QuizQuestion;
using Application.Services.Interfaces.IRepository.Lesons;
using Application.Services.Interfaces.IServices.Lesons;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementations.Lessons
{
    public class LessonQuizQuestionService : IQuizQuestionService
    {
        private readonly IQuizQuestionRepository _quizQuestionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<LessonQuizQuestionService> _logger;

        public LessonQuizQuestionService(IQuizQuestionRepository quizQuestionRepository, IMapper mapper,
            ILogger<LessonQuizQuestionService> logger)
        {
            _quizQuestionRepository = quizQuestionRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<QuizQuestionDto> CreateQuizQuestionAsync(CreateQuizQuestionDto dto)
        {
            try
            {
                _logger.LogInformation("Creating new quiz question for quiz {QuizId}", dto.QuizId);

                var quizExists = await _quizQuestionRepository.QuizExistsAsync(dto.QuizId);
                if (!quizExists)
                {
                    throw new NotFoundException($"Quiz with ID {dto.QuizId} not found", "QUIZ_NOT_FOUND");
                }

                var quizQuestion = _mapper.Map<QuizQuestion>(dto);

                var createdQuestion = await _quizQuestionRepository.CreateQuizQuestionAsync(quizQuestion);

                _logger.LogInformation("Successfully created quiz question with ID {QuestionId}", createdQuestion.Id);
                
                return _mapper.Map<QuizQuestionDto>(createdQuestion);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation error while creating quiz question");
                throw new ServiceException(ex.Message, "VALIDATION_ERROR", ex);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Quiz not found while creating question");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating quiz question");
                throw new ServiceException("Failed to create quiz question", "CREATE_ERROR", ex);
            }
        }

        public async Task<QuizQuestionDto> GetQuizQuestionByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Getting quiz question by ID {QuestionId}", id);

                var question = await _quizQuestionRepository.GetQuizQuestionByIdAsync(id);
                if (question == null)
                {
                    throw new NotFoundException($"Quiz question with ID {id} not found", "QUESTION_NOT_FOUND");
                }

                return _mapper.Map<QuizQuestionDto>(question);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Quiz question not found");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting quiz question by ID {QuestionId}", id);
                throw new ServiceException("Failed to get quiz question", "QUERY_ERROR", ex);
            }
        }

        public async Task<IEnumerable<QuizQuestionDto>> GetAllQuizQuestionsAsync()
        {
            try
            {
                _logger.LogInformation("Getting all quiz questions");

                var questions = await _quizQuestionRepository.GetQuizQuestionAllAsync();
                
                return _mapper.Map<IEnumerable<QuizQuestionDto>>(questions);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "No quiz questions found");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all quiz questions");
                throw new ServiceException("Failed to get quiz questions", "QUERY_ERROR", ex);
            }
        }

        public async Task<QuizQuestion> UpdateQuizQuestionAsync(UpdateQuizQuestionDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("Attempted to update with null DTO");
                throw new ArgumentNullException(nameof(dto));
            }

            try
            {
                return await _quizQuestionRepository.UpdateQuizQuestionAsync(dto);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "QuizQuestion not found for update");
                throw;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error updating quiz question");
                throw new DatabaseException("Failed to update quiz question", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error updating quiz question");
                throw new ApplicationException("Update operation failed", ex);
            }
        }

        public async Task DeleteQuizQuestionAsync(int id)
        {
            try
            {
                _logger.LogInformation("Deleting quiz question with ID {QuestionId}", id);

                await _quizQuestionRepository.DeleteQuizQuestionAsync(id);

                _logger.LogInformation("Successfully deleted quiz question with ID {QuestionId}", id);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Quiz question not found for deletion");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting quiz question with ID {QuestionId}", id);
                throw new ServiceException("Failed to delete quiz question", "DELETE_ERROR", ex);
            }
        }

        public async Task<IEnumerable<QuizQuestionDto>> GetQuizQuestionsByQuizIdAsync(int quizId)
        {
            try
            {
                _logger.LogInformation("Getting quiz questions for quiz ID {QuizId}", quizId);

                var quizExists = await _quizQuestionRepository.QuizExistsAsync(quizId);
                if (!quizExists)
                {
                    throw new NotFoundException($"Quiz with ID {quizId  } not found", "QUIZ_NOT_FOUND");
                }

                var questions = await _quizQuestionRepository.GetByQuizIdAsync(quizId);
                
                return _mapper.Map<IEnumerable<QuizQuestionDto>>(questions);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Quiz or questions not found");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting quiz questions for quiz ID {QuizId}", quizId);
                throw new ServiceException("Failed to get quiz questions", "QUERY_ERROR", ex);
            }
        }
    }
}