using Main = Domain.Entities.Metas;

namespace Application.Interface.Services
{
    public interface IMetasService : IDisposable
    {
        Task<IEnumerable<Main>> GetAll();
        Task<bool> ExecuteProcess();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
        Task<Main> GetById(int id);
        Task<Main> Add(Main entity);
        Task<Main> Update(Main entity);
        Task<bool> DeleteById(int id);

    }
}
