using Application.DtoModels.KidQuiz.KidWordCard;
using Application.DtoModels.KidQuiz;
using AutoMapper;
using Domain.Models;
using Microsoft.Extensions.Logging;
using Application.UnitOfWork;
using Application.Services.Interfaces.IServices.KidQuiz;

namespace Application.Services.Implementations.KidQuiz
{
    public class KidWordCardService : IKidWordCardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<KidWordCardService> _logger;

        public KidWordCardService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<KidWordCardService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<KidWordCardDto?> GetWordCardByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Starting retrieval of KidWordCard with ID {KidWordCardId}", id);

                var wordCard = await _unitOfWork.KidWordCardRepository.GetByIdAsync(id);
                if (wordCard == null)
                {
                    _logger.LogWarning("KidWordCard with ID {KidWordCardId} not found", id);
                    return null;
                }

                _logger.LogInformation("Successfully retrieved KidWordCard with ID {KidWordCardId}", id);
                return _mapper.Map<KidWordCardDto>(wordCard);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving KidWordCard with ID {KidWordCardId}", id);
                return null;
            }
        }

        public async Task<IEnumerable<KidWordCardDto>> GetAllWordCardsAsync()
        {
            try
            {
                _logger.LogInformation("Starting retrieval of all KidWordCards");

                var wordCards = await _unitOfWork.KidWordCardRepository.GetAllAsync();

                _logger.LogInformation("Successfully retrieved all KidWordCards, total count: {Count}", wordCards.Count());
                return _mapper.Map<IEnumerable<KidWordCardDto>>(wordCards);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all KidWordCards");
                return Enumerable.Empty<KidWordCardDto>();
            }
        }

        public async Task<KidWordCardDto?> CreateWordCardAsync(CreateKidWordCardDto dto)
        {
            try
            {
                _logger.LogInformation("Starting creation of KidWordCard for KidLesson ID {KidLessonId}", dto.KidLessonId);

                var lesson = await _unitOfWork.KidLessonRepository.GetLessonByIdAsync(dto.KidLessonId);
                if (lesson == null)
                {
                    var message = $"KidLesson with ID {dto.KidLessonId} not found.";
                    _logger.LogWarning(message);
                    throw new KeyNotFoundException(message);
                }

                var wordCard = _mapper.Map<KidWordCard>(dto);
                var createdWordCard = await _unitOfWork.KidWordCardRepository.AddAsync(wordCard);

                _logger.LogInformation("Successfully created KidWordCard with ID {KidWordCardId} for KidLesson ID {KidLessonId}", createdWordCard.Id, dto.KidLessonId);

                return _mapper.Map<KidWordCardDto>(createdWordCard);
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while creating KidWordCard for KidLesson ID {KidLessonId}", dto.KidLessonId);
                throw;
            }
        }

        public async Task<KidWordCardDto?> UpdateWordCardAsync(int id, UpdateKidWordCardDto dto)
        {
            try
            {
                _logger.LogInformation("Starting update of KidWordCard with ID {KidWordCardId}", id);

                var wordCard = _mapper.Map<KidWordCard>(dto);
                wordCard.Id = id;

                var updatedCard = await _unitOfWork.KidWordCardRepository.UpdateAsync(wordCard);
                if (updatedCard == null)
                {
                    _logger.LogWarning("KidWordCard with ID {KidWordCardId} not found for update", id);
                    return null;
                }

                _logger.LogInformation("Successfully updated KidWordCard with ID {KidWordCardId}", id);
                return _mapper.Map<KidWordCardDto>(updatedCard);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating KidWordCard with ID {KidWordCardId}", id);
                return null;
            }
        }

        public async Task<bool> DeleteWordCardAsync(int id)
        {
            try
            {
                _logger.LogInformation("Starting deletion of KidWordCard with ID {KidWordCardId}", id);

                var result = await _unitOfWork.KidWordCardRepository.DeleteAsync(id);
                if (result)
                {
                    _logger.LogInformation("Successfully deleted KidWordCard with ID {KidWordCardId}", id);
                }
                else
                {
                    _logger.LogWarning("KidWordCard with ID {KidWordCardId} not found for deletion", id);
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting KidWordCard with ID {KidWordCardId}", id);
                return false;
            }
        }

        public async Task<IEnumerable<KidWordCardDto>> GetWordCardsByLessonIdAsync(int lessonId)
        {
            try
            {
                _logger.LogInformation("Starting retrieval of KidWordCards for KidLesson ID {KidLessonId}", lessonId);

                var wordCards = await _unitOfWork.KidWordCardRepository.GetWordCardsByLessonIdAsync(lessonId);

                _logger.LogInformation("Successfully retrieved {Count} KidWordCards for KidLesson ID {KidLessonId}", wordCards.Count(), lessonId);
                return _mapper.Map<IEnumerable<KidWordCardDto>>(wordCards);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving KidWordCards for KidLesson ID {KidLessonId}", lessonId);
                return Enumerable.Empty<KidWordCardDto>();
            }
        }
    }
}
