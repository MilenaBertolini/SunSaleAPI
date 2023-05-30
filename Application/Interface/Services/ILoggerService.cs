using Main = Domain.Entities.Logger;

namespace Application.Interface.Services
{
    public interface ILoggerService : IDisposable
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
        Task<Main> GetById(int id);
        Task<Main> Add(Main entity);
        Task<Main> AddException(Exception exception);
        Task<Main> AddInfo(string info);
        Task<Main> Update(Main entity);
        Task<bool> DeleteById(int id);

    }
}
