using Main = Domain.Entities.SavedResultsWpp;

namespace Application.Interface.Services
{
    public interface ISavedResultsWppService : IDisposable
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
        Task<Main> GetById(int id);
        Task<Main> GetByToken(string token);
        Task<Main> Add(Main entity);
        Task<Main> Update(Main entity);
        Task<bool> DeleteById(int id);

    }
}
