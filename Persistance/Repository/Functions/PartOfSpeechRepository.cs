using Application.Services.Interfaces.IRepository.Functions;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Persistance.Repository.Functions
{
    public class PartOfSpeechRepository : IPartOfSpeechRepository
    {
        private readonly LanguageLearningDbContext _context;
        private readonly ILogger<PartOfSpeechRepository> _logger;

        public PartOfSpeechRepository(LanguageLearningDbContext context, ILogger<PartOfSpeechRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<PartOfSpeech?> GetByIPartOfSpeechdAsync(int id)
        {
            return await _context.PartOfSpeechs.FindAsync(id);
        }

        public async Task<IEnumerable<PartOfSpeech>> GetAllPartOfSpeechAsync()
        {
            return await _context.PartOfSpeechs.ToListAsync();
        }

        public async Task<PartOfSpeech> AddPartOfSpeechAsync(PartOfSpeech partOfSpeech)
        {
            try
            {
                _context.PartOfSpeechs.Add(partOfSpeech);
                await _context.SaveChangesAsync();
                return partOfSpeech;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding PartOfSpeech");
                throw;
            }
        }

        public async Task UpdatePartOfSpeechAsync(PartOfSpeech entity)
        {
            try
            {
                var existing = await _context.PartOfSpeechs.FindAsync(entity.Id);
                if (existing == null)
                    throw new KeyNotFoundException($"PartOfSpeech with ID {entity.Id} not found");

                if (!string.IsNullOrEmpty(entity.Name))
                    existing.Name = entity.Name;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating PartOfSpeech with ID {Id}", entity.Id);
                throw;
            }
        }

        public async Task<bool> DeletePartOfSpeechAsync(int id)
        {
            try
            {
                var entity = await _context.PartOfSpeechs.FindAsync(id);
                if (entity == null)
                    return false;

                _context.PartOfSpeechs.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting PartOfSpeech with ID {Id}", id);
                throw;
            }
        }
    }

}
