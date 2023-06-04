using Entities.DbSet;
using Entities.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Repository;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;
    protected DbSet<T> _dbSet;
    protected readonly ILogger _logger;

    public GenericRepository(ApplicationDbContext context, ILogger logger)
    {
        _context = context;
        _dbSet = _context.Set<T>();
        _logger = logger;
    }

    public async Task<bool> AddAsync(T entity)
    {
        try
        {
            var result = await _dbSet.AddAsync(entity);
            return true;
        }
        catch(Exception ex) 
        {
            _logger.LogError(ex.Message);
            return false;
        }
    }

    public bool DeleteAsync(T entity)
    {
        try
        {
            _dbSet.Remove(entity);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        try
        {
            return await _dbSet.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Enumerable.Empty<T>();
        }
    }

    public async Task<T?> GetByIdAsync(string id)
    {
        try
        {
            var res = await _dbSet.FindAsync(id);
            return res;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return null;
        }
    }

    public bool UpdateAsync(T entity)
    {
        try
        {
            _dbSet.Update(entity);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return false;
        }
    }
}
