using Application.DtoModels.MainQuestions;
using Application.Services.Interfaces.IRepository.MainQuestions;
using Application.Services.Interfaces.IServices.MainQuestions;
using Application.UnitOfWork;
using AutoMapper;
using Domain.Models;
using Microsoft.Extensions.Logging;

namespace Application.Services.Implementations.MainQuestions
{
    public class MainQuestionService : IMainQuestionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<MainQuestionService> _logger;
        private readonly IMapper _mapper;

        public MainQuestionService(IUnitOfWork unitOfWork, ILogger<MainQuestionService> logger,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MainQuestionDto>> GetAllCategoriesAsync()
        {
            try
            {
                var categories = await _unitOfWork.MainQuestionRepository.GetAllCategoriesAsync();
                
                return _mapper.Map<IEnumerable<MainQuestionDto>>(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all categories");
                throw;
            }
        }

        public async Task<MainQuestionDto?> GetCategoryByIdAsync(int id)
        {
            try
            {
                var category = await _unitOfWork.MainQuestionRepository.GetCategoryByIdAsync(id);
                
                return category == null ? null : _mapper.Map<MainQuestionDto>(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting category by ID: {Id}", id);
                throw;
            }
        }

        public async Task<MainQuestionDto?> CreateCategoryAsync(CreateMainQuestionDto dto)
        {
            try
            {
                if (dto == null)
                {
                    _logger.LogWarning("Attempted to create category with null data");
                    return null;
                }

                var entity = _mapper.Map<MainQuestion>(dto);
                var created = await _unitOfWork.MainQuestionRepository.CreateCategoryAsync(entity);
                
                return created == null ? null : _mapper.Map<MainQuestionDto>(created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating category");
                throw;
            }
        }

        public async Task<MainQuestionDto?> UpdateCategoryAsync(int id, UpdateMainQuestionDto dto)
        {
            try
            {
                if (dto == null)
                {
                    _logger.LogWarning("Attempted to update category with null data");
                    return null;
                }

                var entity = _mapper.Map<MainQuestion>(dto);
                var updated = await _unitOfWork.MainQuestionRepository.UpdateCategoryAsync(id, entity);
                
                return updated == null ? null : _mapper.Map<MainQuestionDto>(updated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating category ID: {Id}", id);
                throw;
            }
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            try
            {
                return await _unitOfWork.MainQuestionRepository.DeleteCategoryAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting category ID: {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<MainQuestionWordDto>> GetWordsByCategoryIdAsync(int categoryId)
        {
            try
            {
                var words = await _unitOfWork.MainQuestionRepository.GetWordsByCategoryIdAsync(categoryId);
                
                return _mapper.Map<IEnumerable<MainQuestionWordDto>>(words);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting words for category ID: {CategoryId}", categoryId);
                throw;
            }
        }

        public async Task<MainQuestionWordDto?> AddWordToMainQuestionAsync(CreateMainQuestionWordDto dto)
        {
            try
            {
                if (dto == null)
                {
                    _logger.LogWarning("Attempted to add null word");
                    return null;
                }

                var word = _mapper.Map<MainQuestionWord>(dto);
                var created = await _unitOfWork.MainQuestionRepository.AddWordToMainQuestionAsync(dto.MainQuestionId, word);
                
                return created == null ? null : _mapper.Map<MainQuestionWordDto>(created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding word to question ID: {MainQuestionId}", dto?.MainQuestionId);
                throw;
            }
        }

        public async Task<bool> UpdateWordAsync(int wordId, UpdateMainQuestionWordDto dto)
        {
            try
            {
                if (dto == null)
                {
                    _logger.LogWarning("Attempted to update word with null data");
                    return false;
                }

                var word = _mapper.Map<MainQuestionWord>(dto);
                
                return await _unitOfWork.MainQuestionRepository.UpdateWordAsync(wordId, word);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating word ID: {WordId}", wordId);
                throw;
            }
        }

        public async Task<bool> DeleteWordAsync(int wordId)
        {
            try
            {
                return await _unitOfWork.MainQuestionRepository.DeleteWordAsync(wordId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting word ID: {WordId}", wordId);
                throw;
            }
        }
    }
}