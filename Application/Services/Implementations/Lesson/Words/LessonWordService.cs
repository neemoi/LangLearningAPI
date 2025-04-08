using Application.DtoModels.Lessons.Words;
using Application.Services.Interfaces.IServices.Lesons;
using Application.UnitOfWork;
using AutoMapper;
using Domain.Models;
using LangLearningAPI.Exceptions;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Application.Services.Implementations.Lesson.Words
{
    public class LessonWordService : ILessonWordService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<LessonWordService> _logger;

        public LessonWordService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<LessonWordService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<LessonWordDto>> GetAllWordsAsync()
        {
            try
            {
                var words = await _unitOfWork.LessonWordRepository.GetAllWordsAsync();
                return _mapper.Map<IEnumerable<LessonWordDto>>(words);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all lesson words");
                throw new ApiException(
                    statusCode: (int)HttpStatusCode.InternalServerError,
                    message: "Failed to retrieve lesson words",
                    errorCode: "LESSON_WORDS_GET_ALL_ERROR",
                    innerException: ex);
            }
        }

        public async Task<LessonWordDto?> GetWordByIdAsync(int id)
        {
            try
            {
                ValidateId(id);

                var word = await _unitOfWork.LessonWordRepository.GetWordByIdAsync(id);
                if (word == null)
                {
                    throw new NotFoundException(
                        $"Lesson word with ID {id} not found",
                        "LESSON_WORD_NOT_FOUND");
                }

                return _mapper.Map<LessonWordDto>(word);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving lesson word with ID {id}");
                throw new ApiException(
                    statusCode: (int)HttpStatusCode.InternalServerError,
                    message: $"Failed to retrieve lesson word with ID {id}",
                    errorCode: "LESSON_WORD_GET_ERROR",
                    innerException: ex);
            }
        }

        public async Task<LessonWordDto> CreateWordAsync(CreateLessonWordDto dto)
        {
            try
            {
                ValidateCreateDto(dto);
                await ValidateBusinessRulesAsync(dto);

                var wordToCreate = _mapper.Map<LessonWord>(dto);
                var createdWord = await _unitOfWork.LessonWordRepository.CreateWordsAsync(wordToCreate);

                _logger.LogInformation("Successfully created word with ID {WordId}", createdWord.Id);
                return _mapper.Map<LessonWordDto>(createdWord);
            }
            catch (Exception ex) when (ex is not ApiException)
            {
                _logger.LogError(ex, "Error creating lesson word");
                throw new ApiException(
                    statusCode: (int)HttpStatusCode.InternalServerError,
                    message: "Failed to create lesson word",
                    errorCode: "LESSON_WORD_CREATE_ERROR",
                    innerException: ex);
            }
            catch (Exception ex)
            {
                throw new ConflictException(
                    $"Word '{dto.Name}' already exists in lesson {dto.LessonId}",
                    "DUPLICATE_WORD",
                    ex);
            }
        }

        public async Task<LessonWordDto> UpdatePartialWordsAsync(int id, UpdateLessonWordDto dto)
        {
            try
            {
                ValidateId(id);
                ValidateUpdateDto(dto);

                var existingWord = await _unitOfWork.LessonWordRepository.GetWordByIdAsync(id);
                if (existingWord == null)
                {
                    throw new NotFoundException(
                        $"Lesson word with ID {id} not found",
                        "LESSON_WORD_NOT_FOUND");
                }

                _mapper.Map(dto, existingWord);
                var updatedWord = await _unitOfWork.LessonWordRepository.UpdateWordsAsync(existingWord);

                _logger.LogInformation("Successfully updated word with ID {WordId}", id);
                return _mapper.Map<LessonWordDto>(updatedWord);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating lesson word with ID {id}");
                throw new ApiException(
                    statusCode: (int)HttpStatusCode.InternalServerError,
                    message: $"Failed to update lesson word with ID {id}",
                    errorCode: "LESSON_WORD_UPDATE_ERROR",
                    innerException: ex);
            }
        }

        public async Task<LessonWordDto> DeleteWordsAsync(int id)
        {
            try
            {
                ValidateId(id);

                var word = await _unitOfWork.LessonWordRepository.GetWordByIdAsync(id);
                if (word == null)
                {
                    throw new NotFoundException(
                        $"Lesson word with ID {id} not found",
                        "LESSON_WORD_NOT_FOUND");
                }

                var deletedWord = await _unitOfWork.LessonWordRepository.DeleteWordsAsync(id);

                _logger.LogInformation("Successfully deleted word with ID {WordId}", id);
                return _mapper.Map<LessonWordDto>(deletedWord);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting lesson word with ID {id}");
                throw new ApiException(
                    statusCode: (int)HttpStatusCode.InternalServerError,
                    message: $"Failed to delete lesson word with ID {id}",
                    errorCode: "LESSON_WORD_DELETE_ERROR",
                    innerException: ex);
            }
        }


        #region Private Methods

        private void ValidateId(int id)
        {
            if (id <= 0)
            {
                throw new ValidationException(
                    message: "Invalid ID",
                    errorCode: "INVALID_ID",
                    errors: new Dictionary<string, string[]>
                    {
                        { "id", new[] { "ID must be greater than 0" } }
                    });
            }
        }

        private void ValidateCreateDto(CreateLessonWordDto dto)
        {
            if (dto == null)
            {
                throw new ValidationException(
                    message: "Create DTO cannot be null",
                    errorCode: "NULL_DTO");
            }

            var errors = new Dictionary<string, string[]>();

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                errors.Add(nameof(dto.Name), new[] { "Word name is required" });
            }
            else if (dto.Name.Length > 100)
            {
                errors.Add(nameof(dto.Name), new[] { "Word name cannot exceed 100 characters" });
            }

            if (string.IsNullOrWhiteSpace(dto.Translation))
            {
                errors.Add(nameof(dto.Translation), new[] { "Translation is required" });
            }

            if (dto.LessonId <= 0)
            {
                errors.Add(nameof(dto.LessonId), new[] { "Valid lesson ID is required" });
            }

            if (errors.Any())
            {
                throw new ValidationException(
                    message: "Validation failed for word creation",
                    errorCode: "WORD_VALIDATION_ERROR",
                    errors: errors);
            }
        }

        private async Task ValidateBusinessRulesAsync(CreateLessonWordDto dto)
        {
            var errors = new Dictionary<string, string[]>();

            if (errors.Any())
            {
                throw new ValidationException(
                    message: "Business validation failed",
                    errorCode: "BUSINESS_VALIDATION_ERROR",
                    errors: errors);
            }
        }

        private void ValidateUpdateDto(UpdateLessonWordDto dto)
        {
            if (dto == null)
            {
                throw new ValidationException(
                    message: "Update DTO cannot be null",
                    errorCode: "NULL_DTO");
            }

            var errors = new Dictionary<string, string[]>();

            if (!string.IsNullOrEmpty(dto.Name) && dto.Name.Length > 100)
            {
                errors.Add(nameof(dto.Name), new[] { "Word name cannot exceed 100 characters" });
            }

            if (!string.IsNullOrEmpty(dto.Translation) && dto.Translation.Length > 100)
            {
                errors.Add(nameof(dto.Translation), new[] { "Translation cannot exceed 100 characters" });
            }

            if (errors.Any())
            {
                throw new ValidationException(
                    message: "Validation failed for word update",
                    errorCode: "WORD_UPDATE_VALIDATION_ERROR",
                    errors: errors);
            }
        }
        #endregion
    }
}