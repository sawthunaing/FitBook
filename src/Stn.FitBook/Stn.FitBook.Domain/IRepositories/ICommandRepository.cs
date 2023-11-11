namespace Stn.FitBook.Domain.IRepositories
{
    public interface ICommandRepository<T> where T : class
    {
        Task CreateAsync(T obj);
        Task UpdateAsync(T obj);
    }
}
