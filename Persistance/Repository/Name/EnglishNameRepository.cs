using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Application.Services.Interfaces.IRepository.Name;

namespace Persistance.Repository.Name
{
    public class EnglishNameRepository : IEnglishNameRepository
    {
        private readonly LanguageLearningDbContext _context;
        private readonly ILogger<EnglishNameRepository> _logger;

        public EnglishNameRepository(LanguageLearningDbContext context, ILogger<EnglishNameRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<EnglishName>> GetAllEnglishNamesAsync()
        {
            try
            {
                return await _context.EnglishNames
                    .Include(e => e.FemaleNames)
                    .Include(e => e.MaleNames)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all English names.");
                throw;
            }
        }

        public async Task<EnglishName?> GetEnglishNameByIdAsync(int id)
        {
            try
            {
                return await _context.EnglishNames
                    .Include(e => e.FemaleNames)
                    .Include(e => e.MaleNames)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => e.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving English name by ID {Id}", id);
                throw;
            }
        }

        public async Task<EnglishName?> CreateEnglishNameAsync(EnglishName name)
        {
            try
            {
                _context.EnglishNames.Add(name);
                await _context.SaveChangesAsync();
                
                return name;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating English name.");
                throw;
            }
        }

        public async Task<EnglishName?> UpdateEnglishNameAsync(int id, EnglishName updatedName)
        {
            try
            {
                var existing = await _context.EnglishNames.FindAsync(id);
                if (existing == null)
                {
                    _logger.LogWarning("English name with ID {Id} not found", id);
                    return null;
                }

                existing.Name = updatedName.Name;
                existing.ImagePath = updatedName.ImagePath;

                await _context.SaveChangesAsync();
                
                return existing;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating English name ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> DeleteEnglishNameAsync(int id)
        {
            try
            {
                var entity = await _context.EnglishNames.FindAsync(id);
                if (entity == null)
                {
                    _logger.LogWarning("English name with ID {Id} not found", id);
                    return false;
                }

                _context.EnglishNames.Remove(entity);
                await _context.SaveChangesAsync();
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting English name ID {Id}", id);
                throw;
            }
        }
    }
}
