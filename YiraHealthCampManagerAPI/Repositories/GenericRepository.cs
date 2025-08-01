using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YiraHealthCampManagerAPI.Context;
using YiraHealthCampManagerAPI.Interfaces.RepositoryInterfaces;

namespace YiraHealthCampManagerAPI.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly YiraDbContext _context;
        public GenericRepository(YiraDbContext context)
        {
            _context = context;
        }

        public async Task<T> Get(int id)
        {
            return await _context.Set<T>().FindAsync(id);
            // this can be from a stored procedure too so 
            // long the T is returning an entity
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        Task<int> IGenericRepository<T>.Add(T entity)
        {
            throw new NotImplementedException();
        }

        Task<int> IGenericRepository<T>.Delete(int entity)
        {
            throw new NotImplementedException();
        }

        Task<int> IGenericRepository<T>.Update(T id)
        {
            throw new NotImplementedException();
        }

        
    }
}
