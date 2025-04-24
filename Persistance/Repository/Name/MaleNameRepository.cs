using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Application.Services.Interfaces.IRepository.Name;

namespace Persistance.Repository.Name
{
    public class MaleNameRepository : IMaleNameRepository
    {
        private readonly LanguageLearningDbContext _context;
        private readonly ILogger<MaleNameRepository> _logger;

        public MaleNameRepository(LanguageLearningDbContext context, ILogger<MaleNameRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<MaleName>> GetAllMaleNamesAsync()
        {
            try
            {
                return await _context.MaleNames
                    .Include(m => m.EnglishName)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all Male names.");
                throw;
            }
        }

        public async Task<MaleName?> GetMaleNameByIdAsync(int id)
        {
            try
            {
                return await _context.MaleNames
                    .Include(m => m.EnglishName)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Male name by ID {Id}", id);
                throw;
            }
        }

        public async Task<MaleName?> CreateMaleNameAsync(MaleName name)
        {
            try
            {
                _context.MaleNames.Add(name);
                await _context.SaveChangesAsync();
                
                return name;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Male name.");
                throw;
            }
        }

        public async Task<MaleName?> UpdateMaleNameAsync(int id, MaleName updatedName)
        {
            try
            {
                var entity = await _context.MaleNames
                    .Include(m => m.EnglishName)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (entity == null)
                {
                    _logger.LogWarning("Male name with ID {Id} not found", id);
                    return null;
                }

                entity.EnglishNameId = updatedName.EnglishNameId;
                entity.Name = updatedName.Name;

                await _context.SaveChangesAsync();
                
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Male name ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> DeleteMaleNameAsync(int id)
        {
            try
            {
                var entity = await _context.MaleNames.FindAsync(id);
                if (entity == null)
                {
                    _logger.LogWarning("Male name with ID {Id} not found", id);
                    return false;
                }

                _context.MaleNames.Remove(entity);
                await _context.SaveChangesAsync();
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Male name ID {Id}", id);
                throw;
            }
        }
    }
}
