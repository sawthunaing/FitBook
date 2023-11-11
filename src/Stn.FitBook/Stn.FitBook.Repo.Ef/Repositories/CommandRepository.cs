using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stn.FitBook.Domain.IRepositories;
using Stn.FitBook.Domain.Models.Entities;

namespace Stn.FitBook.Repo.Ef.Repositories
{
    public class CommandRepository<T> : ICommandRepository<T> where T : class
    {
        #region private 
        private readonly DBContext _context;
        #endregion

        public CommandRepository(DBContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(T Obj)
        {
            await _context.AddAsync(Obj);
        }
              

        public async Task UpdateAsync(T Obj)
        {
            _context.Entry(Obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

      
    }
}
