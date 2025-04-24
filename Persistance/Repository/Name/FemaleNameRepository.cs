using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Application.Services.Interfaces.IRepository.Name;

namespace Persistance.Repository.Name
{
    public class FemaleNameRepository : IFemaleNameRepository
    {
        private readonly LanguageLearningDbContext _context;
        private readonly ILogger<FemaleNameRepository> _logger;

        public FemaleNameRepository(LanguageLearningDbContext context, ILogger<FemaleNameRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<FemaleName>> GetAllFemaleNamesAsync()
        {
            try
            {
                return await _context.FemaleNames
                    .Include(f => f.EnglishName)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all Female names.");
                throw;
            }
        }

        public async Task<FemaleName?> GetFemaleNameByIdAsync(int id)
        {
            try
            {
                return await _context.FemaleNames
                    .Include(fn => fn.EnglishName)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(fn => fn.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting Female name by ID {Id}", id);
                throw;
            }
        }

        public async Task<FemaleName?> CreateFemaleNameAsync(FemaleName name)
        {
            try
            {
                _context.FemaleNames.Add(name);
                await _context.SaveChangesAsync();
                return name;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Female name.");
                throw;
            }
        }

        public async Task<FemaleName?> UpdateFemaleNameAsync(int id, FemaleName updatedName)
        {
            try
            {
                var entity = await _context.FemaleNames
                    .Include(f => f.EnglishName)
                    .FirstOrDefaultAsync(f => f.Id == id);

                if (entity == null)
                {
                    _logger.LogWarning("Female name with ID {Id} not found", id);
                    return null;
                }

                entity.EnglishNameId = updatedName.EnglishNameId;
                entity.Name = updatedName.Name;

                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Female name ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> DeleteFemaleNameAsync(int id)
        {
            try
            {
                var entity = await _context.FemaleNames.FindAsync(id);
                if (entity == null)
                {
                    _logger.LogWarning("Female name with ID {Id} not found", id);
                    return false;
                }

                _context.FemaleNames.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Female name ID {Id}", id);
                throw;
            }
        }
    }
}