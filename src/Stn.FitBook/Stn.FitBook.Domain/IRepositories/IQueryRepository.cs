using System.Threading.Tasks;

namespace Stn.FitBook.Domain.IRepositories
{
    public interface IQueryRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAsync();
        Task<List<T>> GetAllAsync(Func<T, bool> predicate);
        Task<T> GetByIdAsync(Func<T, bool> predicate);
    }
}
