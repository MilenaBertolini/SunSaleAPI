using Main = Domain.Entities.Logger;

namespace Application.Interface.Services
{
    public interface ILoggerService : IDisposable
    {
        Task<IEnumerable<Main>> GetAll();
        Task<Tuple<IEnumerable<Main>, int>> GetAllPagged(int page, int quantity, string message);
        Task<Main> GetById(int id);
        Task<Main> Add(Main entity);
        Task<Main> AddException(Exception exception);
        Task<Main> AddInfo(string info);
        Task<Main> Update(Main entity);
        Task<bool> DeleteById(int id);
        Task<int> QuantidadeTotal();
    }
}
