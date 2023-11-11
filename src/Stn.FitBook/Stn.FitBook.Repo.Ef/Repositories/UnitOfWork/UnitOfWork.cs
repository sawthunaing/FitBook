using Stn.FitBook.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stn.FitBook.Repo.Ef.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DBContext _context;
        public UnitOfWork(DBContext context)
        {
            _context = context;
        }

        public async Task CommitAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                await _context.DisposeAsync();
                throw new ArgumentOutOfRangeException();
            }
        }

        public async Task RollbackAsync() => await _context.DisposeAsync();

        public void Dispose() => _context.Dispose();
    }
}
