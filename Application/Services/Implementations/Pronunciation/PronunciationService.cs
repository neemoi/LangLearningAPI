using Application.DtoModels.Pronunciation;
using Application.Services.Interfaces.IServices.Pronunciation;
using Application.UnitOfWork;
using AutoMapper;
using Domain.Models;
using Microsoft.Extensions.Logging;

namespace Application.Services.Implementations.Pronunciation
{
    public class PronunciationService : IPronunciationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<PronunciationService> _logger;

        public PronunciationService(IUnitOfWork unitOfWork, IMapper mapper,
            ILogger<PronunciationService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<PronunciationCategoryDto>> GetAllCategoriesAsync()
        {
            try
            {
                var categories = await _unitOfWork.PronunciationRepository.GetAllCategoriesAsync();
                if (categories == null || !categories.Any())
                {
                    _logger.LogWarning("No categories found or the list is empty.");
                    return Enumerable.Empty<PronunciationCategoryDto>();
                }

                return _mapper.Map<IEnumerable<PronunciationCategoryDto>>(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all categories.");
                throw;
            }
        }

        public async Task<PronunciationCategoryDto?> GetCategoryByIdAsync(int id)
        {
            try
            {
                var category = await _unitOfWork.PronunciationRepository.GetCategoryByIdAsync(id);
                if (category == null)
                {
                    _logger.LogWarning("Category with Id {Id} not found", id);
                    return null;
                }

                return _mapper.Map<PronunciationCategoryDto>(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving category by Id.");
                throw;
            }
        }

        public async Task<PronunciationCategoryDto?> CreateCategoryAsync(CreateCategoryDto dto)
        {
            try
            {
                var entity = _mapper.Map<PronunciationCategory>(dto);
                if (entity == null)
                {
                    _logger.LogWarning("Mapping CreateCategoryDto to PronunciationCategory failed.");
                    return null;
                }

                var created = await _unitOfWork.PronunciationRepository.CreateCategoryAsync(entity);
                if (created == null)
                {
                    _logger.LogWarning("Failed to create category.");
                    return null;
                }

                return _mapper.Map<PronunciationCategoryDto>(created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating category.");
                throw;
            }
        }

        public async Task<PronunciationCategoryDto?> UpdateCategoryAsync(UpdateCategoryDto dto)
        {
            try
            {
                var existing = await _unitOfWork.PronunciationRepository.GetCategoryByIdAsync(dto.Id);
                if (existing == null)
                {
                    _logger.LogWarning("Category with Id {Id} not found", dto.Id);
                    return null;
                }

                _mapper.Map(dto, existing);

                var success = await _unitOfWork.PronunciationRepository.UpdateCategoryAsync(dto.Id, existing);
                if (!success)
                {
                    _logger.LogWarning("Failed to update category with Id {Id}", dto.Id);
                    return null;
                }

                return _mapper.Map<PronunciationCategoryDto>(existing);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating category.");
                throw;
            }
        }

        public async Task<PronunciationCategoryDto?> DeleteCategoryAsync(int id)
        {
            try
            {
                var existing = await _unitOfWork.PronunciationRepository.GetCategoryByIdAsync(id);
                if (existing == null)
                {
                    _logger.LogWarning("Category with Id {Id} not found", id);
                    return null;
                }

                var success = await _unitOfWork.PronunciationRepository.DeleteCategoryAsync(id);
                if (!success)
                {
                    _logger.LogWarning("Failed to delete category with Id {Id}", id);
                    return null;
                }

                return _mapper.Map<PronunciationCategoryDto>(existing);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting category.");
                throw;
            }
        }

        public async Task<IEnumerable<WordItemDto>> GetWordsByCategoryIdAsync(int categoryId)
        {
            try
            {
                var words = await _unitOfWork.PronunciationRepository.GetWordsByCategoryIdAsync(categoryId);
                if (words == null || !words.Any())
                {
                    _logger.LogWarning("No words found for category with Id {CategoryId}", categoryId);
                    return Enumerable.Empty<WordItemDto>();
                }

                return _mapper.Map<IEnumerable<WordItemDto>>(words);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving words for category with Id {CategoryId}");
                throw;
            }
        }

        public async Task<WordItemDto?> AddWordToCategoryAsync(int categoryId, CreateWordItemDto dto)
        {
            try
            {
                var entity = _mapper.Map<WordItem>(dto);
                if (entity == null)
                {
                    _logger.LogWarning("Mapping CreateWordItemDto to WordItem failed.");
                    return null;
                }

                var added = await _unitOfWork.PronunciationRepository.AddWordToCategoryAsync(categoryId, entity);
                if (added == null)
                {
                    _logger.LogWarning("Failed to add word to category.");
                    return null;
                }

                return _mapper.Map<WordItemDto>(added);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding word to category.");
                throw;
            }
        }

        public async Task<WordItemDto?> UpdateWordAsync(UpdateWordItemDto dto)
        {
            try
            {
                var word = await _unitOfWork.PronunciationRepository.GetWordByIdAsync(dto.Id);
                if (word == null)
                {
                    _logger.LogWarning("Word with Id {Id} not found", dto.Id);
                    return null;
                }

                if (dto.Name != null)
                    word.Name = dto.Name;

                if (dto.ImagePath != null)
                    word.ImagePath = dto.ImagePath;

                var success = await _unitOfWork.PronunciationRepository.UpdateWordAsync(dto.Id, word);
                if (!success)
                {
                    _logger.LogWarning("Failed to update word with Id {Id}", dto.Id);
                    return null;
                }

                return _mapper.Map<WordItemDto>(word);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating word.");
                throw;
            }
        }

        public async Task<WordItemDto?> DeleteWordAsync(int wordId)
        {
            try
            {
                var existing = await _unitOfWork.PronunciationRepository.GetWordByIdAsync(wordId);
                if (existing == null)
                {
                    _logger.LogWarning("Word with Id {WordId} not found", wordId);
                    return null;
                }

                var success = await _unitOfWork.PronunciationRepository.DeleteWordAsync(wordId);
                if (!success)
                {
                    _logger.LogWarning("Failed to delete word with Id {WordId}", wordId);
                    return null;
                }

                return _mapper.Map<WordItemDto>(existing);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting word.");
                throw;
            }
        }
    }
}