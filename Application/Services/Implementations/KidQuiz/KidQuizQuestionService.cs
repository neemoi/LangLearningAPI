using Application.DtoModels.KidQuiz;
using Application.DtoModels.KidQuiz.KidQuizQuestion;
using Application.Services.Interfaces.IServices.KidQuiz;
using Application.UnitOfWork;
using AutoMapper;
using Domain.Models;
using Microsoft.Extensions.Logging;

namespace Application.Services.Implementations.KidQuiz
{
    public class KidQuizQuestionService : IKidQuizQuestionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<KidQuizQuestionService> _logger;
        private readonly IMapper _mapper;

        public KidQuizQuestionService(IUnitOfWork unitOfWork, ILogger<KidQuizQuestionService> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<KidQuizQuestionDto?> GetByIdQuizQuestionAsync(int id)
        {
            try
            {
                var question = await _unitOfWork.KidQuizQuestionRepository.GetByIdQuizQuestionAsync(id);
                if (question == null)
                {
                    _logger.LogWarning($"KidQuizQuestion with ID {id} not found.");
                    return null;
                }

                return _mapper.Map<KidQuizQuestionDto>(question);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving KidQuizQuestion with ID {id}: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<KidQuizQuestionDto>> GetByLessonIdQuizQuestionAsync(int lessonId)
        {
            try
            {
                var questions = await _unitOfWork.KidQuizQuestionRepository.GetByLessonIdQuizQuestionAsync(lessonId);
                if (!questions.Any())
                {
                    _logger.LogWarning($"No KidQuizQuestions found for LessonId {lessonId}.");
                }

                return questions;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving KidQuizQuestions for LessonId {lessonId}: {ex.Message}");
                throw;
            }
        }

        public async Task<KidQuizQuestionDto?> AddQuizQuestionAsync(CreateKidQuizQuestionDto questionDto)
        {
            try
            {
                var question = _mapper.Map<KidQuizQuestion>(questionDto);
                var addedQuestion = await _unitOfWork.KidQuizQuestionRepository.AddQuizQuestionAsync(question);

                if (addedQuestion == null)
                {
                    _logger.LogWarning("Failed to add KidQuizQuestion.");
                    return null;
                }

                return _mapper.Map<KidQuizQuestionDto>(addedQuestion);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding KidQuizQuestion");
                throw;
            }
        }

        public async Task<KidQuizQuestionDto?> UpdateQuizQuestionAsync(int id, UpdateKidQuizQuestionDto questionDto)
        {
            try
            {
                var existingQuestion = await _unitOfWork.KidQuizQuestionRepository.GetByIdQuizQuestionAsync(id);
                if (existingQuestion == null)
                {
                    _logger.LogWarning("Question with ID {QuestionId} not found", id);
                    return null;
                }

                if (questionDto.QuestionText != null)
                    existingQuestion.QuestionText = questionDto.QuestionText;

                if (questionDto.AudioUrl != null)
                    existingQuestion.AudioUrl = questionDto.AudioUrl;

                if (questionDto.ImageUrl != null)
                    existingQuestion.ImageUrl = questionDto.ImageUrl;

                if (questionDto.CorrectAnswer != null)
                    existingQuestion.CorrectAnswer = questionDto.CorrectAnswer;

                if (questionDto.LessonId.HasValue)
                    existingQuestion.LessonId = questionDto.LessonId.Value;

                if (questionDto.QuizTypeId.HasValue)
                    existingQuestion.QuizTypeId = questionDto.QuizTypeId.Value;

                var updatedQuestion = await _unitOfWork.KidQuizQuestionRepository.UpdateQuizQuestionAsync(existingQuestion);
                if (updatedQuestion == null)
                {
                    _logger.LogError("Failed to update question with ID {QuestionId}", id);
                    return null;
                }

                return _mapper.Map<KidQuizQuestionDto>(updatedQuestion);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating question with ID {QuestionId}", id);
                throw;
            }
        }

        public async Task<KidQuizQuestion?> DeleteQuizQuestionAsync(int id)
        {
            try
            {
                var question = await _unitOfWork.KidQuizQuestionRepository.GetByIdQuizQuestionAsync(id);
                if (question == null)
                {
                    _logger.LogWarning($"KidQuizQuestion with ID {id} not found for deletion.");
                    return null;
                }


                return await _unitOfWork.KidQuizQuestionRepository.DeleteQuizQuestionAstnc(question);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting KidQuizQuestion with ID {id}: {ex.Message}");
                throw;
            }
        }
    }
}
