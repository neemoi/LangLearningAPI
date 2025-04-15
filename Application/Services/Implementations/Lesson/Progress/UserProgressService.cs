using Application.DtoModels.Lessons.Progress;
using Application.Services.Interfaces.IServices.Lesons;
using Application.UnitOfWork;
using AutoMapper;
using Domain.Models;
using LangLearningAPI.Exceptions;
using Microsoft.Extensions.Logging;

namespace Application.Services.Implementations.Lesson.Progress
{
    public class UserProgressService : IUserProgressService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UserProgressService> _logger;

        public UserProgressService(IUnitOfWork unitOfWork, IMapper mapper,
            ILogger<UserProgressService> logger)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<UserProgressCreateUpdateDto> AddUserProgressAsync(UserProgressCreateUpdateDto progressDto)
        {
            try
            {
                var progress = _mapper.Map<UserProgress>(progressDto);
                var result = await _unitOfWork.UserProgressRepository.AddUserProgressAsync(progress);
                
                return _mapper.Map<UserProgressCreateUpdateDto>(result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Progress already exists");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add user progress");
                throw;
            }
        }

        public async Task<UserProgressCreateUpdateDto> UpdateUserProgressAsync(UserProgressCreateUpdateDto progressDto)
        {
            try
            {
                var progress = _mapper.Map<UserProgress>(progressDto);
                var result = await _unitOfWork.UserProgressRepository.UpdateUserProgressAsync(progress);
                
                return _mapper.Map<UserProgressCreateUpdateDto>(result);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Progress not found for update");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update user progress");
                throw;
            }
        }

        public async Task<UserLessonProgressViewDto> GetFullProgressAsync(string userId, int lessonId)
        {
            try
            {
                return await _unitOfWork.UserProgressRepository.GetFullProgressAsync(userId, lessonId);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Progress not found");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting full lesson progress for user");
                throw new ApplicationException("Failed to get detailed progress", ex);
            }
        }

        public async Task<UserWordStatsDto> GetWordStatsAsync(string userId, int lessonId)
        {
            try
            {
                return await _unitOfWork.UserProgressRepository.GetWordStatsAsync(userId, lessonId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting word stats");
                throw;
            }
        }

        public async Task<UserWordProgressResponseDto> AddWordProgressAsync(UserWordProgressCreateDto progressDto)
        {
            try
            {
                var progress = _mapper.Map<Domain.Models.UserWordProgress>(progressDto);

                var result = await _unitOfWork.UserProgressRepository.AddWordProgressAsync(progress);
                var response = _mapper.Map<UserWordProgressResponseDto>(result);

                response.QuestionType = result.QuestionType.ToString();

                return response;
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid QuestionType provided");
                throw new ValidationException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add word progress");
                throw;
            }
        }
        public async Task<UserWordProgressResponseDto> UpdateWordProgressAsync(int id, UserWordProgressCreateDto progressDto)
        {
            try
            {
                var existing = await _unitOfWork.UserProgressRepository.GetWordProgressAsync(id);
                if (existing == null)
                {
                    throw new NotFoundException("Word progress not found");
                }

                _mapper.Map(progressDto, existing);
                var updated = await _unitOfWork.UserProgressRepository.UpdateWordProgressAsync(existing);
                
                return _mapper.Map<UserWordProgressResponseDto>(updated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update word progress {Id}", id);
                throw;
            }
        }

        public async Task<UserWordProgressResponseDto> GetWordProgressAsync(int id)
        {
            try
            {
                var progress = await _unitOfWork.UserProgressRepository.GetWordProgressAsync(id);
                if (progress == null)
                {
                    throw new NotFoundException("Word progress not found");
                }
                
                return _mapper.Map<UserWordProgressResponseDto>(progress);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting word progress {Id}", id);
                throw;
            }
        }

        public async Task<List<UserWordProgressResponseDto>> GetWordProgressesByUserAndLessonAsync(string userId, int lessonId)
        {
            try
            {
                var progresses = await _unitOfWork.UserProgressRepository.GetWordProgressesByUserAndLessonAsync(userId, lessonId);
                
                return _mapper.Map<List<UserWordProgressResponseDto>>(progresses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting word progresses for user {UserId}, lesson {LessonId}", userId, lessonId);
                throw;
            }
        }

        public async Task<UserProgressResponseDto> UpsertProgressAsync(UserProgressCreateUpdateDto progressDto)
        {
            try
            {
                try
                {
                    var added = await AddUserProgressAsync(progressDto);
                    return _mapper.Map<UserProgressResponseDto>(added);
                }
                catch (InvalidOperationException)
                {
                    var updated = await UpdateUserProgressAsync(progressDto);
                    
                    return _mapper.Map<UserProgressResponseDto>(updated);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpsertProgressAsync");
                throw;
            }
        }
    }
}