using Application.DtoModels.KidQuiz.KidQuizAnswer;
using Application.Services.Interfaces.IServices.KidQuiz;
using Application.UnitOfWork;
using AutoMapper;
using Domain.Models;
using Microsoft.Extensions.Logging;

namespace Application.Services.Implementations.KidQuiz
{
    public class KidQuizAnswerService : IKidQuizAnswerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<KidQuizAnswerService> _logger;

        public KidQuizAnswerService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<KidQuizAnswerService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<KidQuizAnswerDto?> GetKidQuizAnswerByIdAsync(int id)
        {
            try
            {
                var answer = await _unitOfWork.KidQuizAnswerRepository.GetByIdKidQuizAnswerAsync(id);
                if (answer == null)
                {
                    _logger.LogWarning($"KidQuizAnswer with Id {id} not found.");
                    return null;
                }

                return _mapper.Map<KidQuizAnswerDto>(answer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving KidQuizAnswer with Id {id}");
                throw;
            }
        }

        public async Task<IEnumerable<KidQuizAnswerDto>> GetAnswersByQuestionIdAsync(int questionId)
        {
            try
            {
                var answers = await _unitOfWork.KidQuizAnswerRepository.GetByQuestionIdAsync(questionId);
                
                return _mapper.Map<IEnumerable<KidQuizAnswerDto>>(answers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving KidQuizAnswers for QuestionId {questionId}");
                throw;
            }
        }

        public async Task<KidQuizAnswerDto?> AddKidQuizAnswerAsync(CreateKidQuizAnswerDto dto)
        {
            try
            {
                var existingQuestion = await _unitOfWork.KidQuizQuestionRepository.GetByIdQuizQuestionAsync(dto.QuestionId);
                if (existingQuestion == null)
                {
                    _logger.LogWarning("Cannot add KidQuizAnswer: Question with ID {QuestionId} does not exist.", dto.QuestionId);
                    return null;
                }

                var answer = _mapper.Map<KidQuizAnswer>(dto);
                var addedAnswer = await _unitOfWork.KidQuizAnswerRepository.AddKidQuizAnswerAsync(answer);

                if (addedAnswer == null)
                {
                    _logger.LogWarning("Failed to add KidQuizAnswer.");
                    return null;
                }

                return _mapper.Map<KidQuizAnswerDto>(addedAnswer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding KidQuizAnswer");
                throw;
            }
        }


        public async Task<KidQuizAnswerDto?> UpdateKidQuizAnswerAsync(UpdateKidQuizAnswerDto dto)
        {
            try
            {
                var existingAnswer = await _unitOfWork.KidQuizAnswerRepository.GetByIdKidQuizAnswerAsync(dto.Id);
                if (existingAnswer == null)
                {
                    _logger.LogWarning($"KidQuizAnswer with Id {dto.Id} not found for update.");
                    return null;
                }

                if (!string.IsNullOrWhiteSpace(dto.AnswerText))
                    existingAnswer.AnswerText = dto.AnswerText;

                if (dto.IsCorrect.HasValue)
                    existingAnswer.IsCorrect = dto.IsCorrect.Value;

                if (dto.QuestionId.HasValue && dto.QuestionId.Value != 0)
                    existingAnswer.QuestionId = dto.QuestionId.Value;

                var updatedAnswer = await _unitOfWork.KidQuizAnswerRepository.UpdateKidQuizAnswerAsync(existingAnswer);

                return _mapper.Map<KidQuizAnswerDto>(updatedAnswer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating KidQuizAnswer with Id {dto.Id}");
                throw;
            }
        }

        public async Task<bool> DeleteKidQuizAnswerAsync(int id)
        {
            try
            {
                var deletedAnswer = await _unitOfWork.KidQuizAnswerRepository.DeleteKidQuizAnswerAsync(id);
                
                return deletedAnswer != null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting KidQuizAnswer with Id {id}");
                throw;
            }
        }
    }
}
