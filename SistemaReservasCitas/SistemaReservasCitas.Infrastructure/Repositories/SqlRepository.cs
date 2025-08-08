using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using SistemaReservasCitas.Domain.Repositories;
using SistemaReservasCitas.Infrastructure.Data;

namespace SistemaReservasCitasApi.Infrastructure.Repositories
{
    public class SqlRepository<T> : IRepository<T> where T : class
    {
        private readonly SistemaReservasCitasContext _context;

        public SqlRepository(SistemaReservasCitasContext context)
        {
            _context = context;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }


        public async Task<T?> FindAsync(Expression<Func<T, bool>> predicate)
        {
                return await _context.Set<T>().FirstOrDefaultAsync(predicate);
         }

  }
}