using Microsoft.EntityFrameworkCore;
using Stn.FitBook.Domain.IRepositories;
using Stn.FitBook.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stn.FitBook.Repo.Ef.Repositories
{
    public class QueryRepository<T> : IQueryRepository<T> where T : class
    {
        #region private
        private readonly DBContext _context;
        #endregion
        public QueryRepository(DBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }
        public async Task<List<T>> GetAllAsync(Func<T, bool> predicate)
        {
            return _context.Set<T>().Where(predicate).ToList();
        }
        public async Task<T> GetByIdAsync(Func<T, bool> predicate)
        {
            return _context.Set<T>().SingleOrDefault(predicate);
        }
              
    }
}
