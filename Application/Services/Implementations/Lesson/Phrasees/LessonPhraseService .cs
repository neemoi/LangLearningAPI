using Application.DtoModels.Lessons.Phrasees;
using Application.Services.Interfaces.IServices.Lesons;
using Application.UnitOfWork;
using AutoMapper;
using Common.Exceptions;
using Domain.Models;
using LangLearningAPI.Exceptions;
using Microsoft.Extensions.Logging;

namespace Application.Services.Implementations.Lesson.Phrasees
{
    public class LessonPhraseService : ILessonPhraseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<LessonPhraseService> _logger;

        public LessonPhraseService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<LessonPhraseService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<LessonPhraseDto> CreatePhraseAsync(CreateLessonPhraseDto dto)
        {
            try
            {
                _logger.LogInformation("Creating new phrase");

                if (dto == null)
                    throw new ArgumentException("Phrase DTO cannot be null");

                if (dto.LessonId <= 0)
                    throw new ArgumentException("Invalid lesson ID");

                var phrase = _mapper.Map<LessonPhrase>(dto);

                var createdPhrase = await _unitOfWork.LessonPhraseRepository.CreatePhraseAsync(phrase);

                return _mapper.Map<LessonPhraseDto>(createdPhrase);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid data when creating phrase");
                throw new ServiceException("Invalid phrase data", "INVALID_DATA", ex);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Lesson not found");
                throw new ServiceException("Lesson not found", "LESSON_NOT_FOUND", ex);
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, "Database error when creating phrase");
                throw new ServiceException("Error while creating phrase in database", "DB_ERROR", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error when creating phrase");
                throw new ServiceException("Unexpected error while creating phrase", "UNEXPECTED_ERROR", ex);
            }
        }

        public async Task<IEnumerable<LessonPhraseDto>> GetAllPhrasesAsync()
        {
            try
            {
                _logger.LogInformation("Getting all phrases");

                var phrases = await _unitOfWork.LessonPhraseRepository.GetAllPhrasesAsync();

                return _mapper.Map<IEnumerable<LessonPhraseDto>>(phrases);
            }
            catch (EmptyCollectionException ex)
            {
                _logger.LogWarning(ex, "No phrases found");
                throw new ServiceException("No phrases available", ex);
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, "Database error when getting phrases");
                throw new ServiceException("Error while retrieving phrases", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error when getting phrases");
                throw new ServiceException("Unexpected error while retrieving phrases", ex);
            }
        }

        public async Task<LessonPhraseDto> GetPhraseByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Getting phrase by ID: {id}");

                var phrase = await _unitOfWork.LessonPhraseRepository.GetPhraseByIdAsync(id);

                return _mapper.Map<LessonPhraseDto>(phrase);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Phrase not found: {id}");
                throw new ServiceException($"Phrase with ID {id} not found", ex);
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, $"Database error when getting phrase: {id}");
                throw new ServiceException($"Error while retrieving phrase with ID {id}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error when getting phrase: {id}");
                throw new ServiceException($"Unexpected error while retrieving phrase with ID {id}", ex);
            }
        }

        public async Task<LessonPhraseDto> DeletePhraseAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting phrase with ID: {id}");

                var deletedPhrase = await _unitOfWork.LessonPhraseRepository.DeletePhraseAsync(id);

                return _mapper.Map<LessonPhraseDto>(deletedPhrase);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Phrase not found for deletion: {id}");
                throw new ServiceException($"Phrase with ID {id} not found for deletion", ex);
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, $"Database error when deleting phrase: {id}");
                throw new ServiceException($"Error while deleting phrase with ID {id}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error when deleting phrase: {id}");
                throw new ServiceException($"Unexpected error while deleting phrase with ID {id}", ex);
            }
        }

        public async Task<LessonPhraseDto> UpdatePhraseAsync(UpdateLessonPhraseDto dto)
        {
            try
            {
                _logger.LogInformation("Updating phrase with ID: {PhraseId}", dto.PhraseId);

                if (dto == null)
                    throw new ArgumentException("Update DTO cannot be null");

                if (dto.PhraseId <= 0)
                    throw new ArgumentException("Invalid phrase ID");

                var updatedPhrase = await _unitOfWork.LessonPhraseRepository.UpdatePhraseAsync(dto);

                return _mapper.Map<LessonPhraseDto>(updatedPhrase);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid data when updating phrase");
                throw new ServiceException("Invalid phrase data", "INVALID_DATA", ex);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Phrase not found");
                throw new ServiceException("Phrase not found", "PHRASE_NOT_FOUND", ex);
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, "Database error when updating phrase");
                throw new ServiceException("Error while updating phrase in database", "DB_ERROR", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error when updating phrase");
                throw new ServiceException("Unexpected error while updating phrase", "UNEXPECTED_ERROR", ex);
            }
        }
    }
}
