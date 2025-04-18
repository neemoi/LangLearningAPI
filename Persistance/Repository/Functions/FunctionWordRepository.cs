using Application.Services.Interfaces.IRepository.Functions;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Persistance.Repository.Functions
{
    public class FunctionWordRepository : IFunctionWordRepository
    {
        private readonly LanguageLearningDbContext _context;
        private readonly ILogger<FunctionWordRepository> _logger;

        public FunctionWordRepository(LanguageLearningDbContext context, ILogger<FunctionWordRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<FunctionWord?> GetByIdFunctionWordAsync(int id)
        {
            try
            {
                return await _context.FunctionWords
                    .Include(fw => fw.PartOfSpeech)
                    .FirstOrDefaultAsync(fw => fw.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving FunctionWord with ID {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<FunctionWord>> GetAllFunctionWordAsync()
        {
            try
            {
                return await _context.FunctionWords
                    .Include(fw => fw.PartOfSpeech)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all FunctionWords");
                throw;
            }
        }

        public async Task<FunctionWord> AddFunctionWordAsync(FunctionWord entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity), "FunctionWord entity cannot be null");

                if (entity.PartOfSpeechId == 0)
                    throw new ArgumentException("PartOfSpeech ID must be provided");

                var partOfSpeechExists = await _context.PartOfSpeechs
                    .AnyAsync(pos => pos.Id == entity.PartOfSpeechId);

                if (!partOfSpeechExists)
                    throw new KeyNotFoundException($"PartOfSpeech with ID {entity.PartOfSpeechId} not found");

                await _context.FunctionWords.AddAsync(entity);
                await _context.SaveChangesAsync();

                return entity; 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding FunctionWord");
                throw;
            }
        }

        public async Task UpdateFunctionWordAsync(FunctionWord entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity), "FunctionWord entity cannot be null");

                if (entity.Id == 0)
                    throw new ArgumentException("FunctionWord ID must not be zero for update");

                var existingEntity = await _context.FunctionWords
                    .FirstOrDefaultAsync(fw => fw.Id == entity.Id);

                if (existingEntity == null)
                {
                    _logger.LogWarning("FunctionWord with ID {Id} not found", entity.Id);
                    throw new KeyNotFoundException($"FunctionWord with ID {entity.Id} not found");
                }

                var partOfSpeechExists = await _context.PartOfSpeechs
                    .AnyAsync(pos => pos.Id == entity.PartOfSpeechId);

                if (!partOfSpeechExists)
                    throw new KeyNotFoundException($"PartOfSpeech with ID {entity.PartOfSpeechId} not found");

                _context.Entry(existingEntity).CurrentValues.SetValues(entity);
                
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating FunctionWord with ID {Id}", entity.Id);
                throw;
            }
        }

        public async Task<bool> DeleteFunctionWordAsync(int id)
        {
            try
            {
                var entity = await GetByIdFunctionWordAsync(id);
                if (entity == null)
                {
                    _logger.LogWarning("FunctionWord with ID {Id} not found for deletion", id);
                    return false;
                }

                _context.FunctionWords.Remove(entity);
                await _context.SaveChangesAsync();
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting FunctionWord with ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> ExistsFunctionWordAsync(int id)
        {
            try
            {
                return await _context.FunctionWords.AnyAsync(fw => fw.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking existence of FunctionWord with ID {Id}", id);
                throw;
            }
        }
    }
}
