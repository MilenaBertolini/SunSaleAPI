using Main = Domain.Entities.RecuperaSenha;

namespace Application.Interface.Services
{
    public interface IRecuperaSenhaService : IDisposable
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
        Task<Main> GetById(int id);
        Task<Main> GetByGuid(string guid);
        Task<Main> Add(Main entity);
        Task<Main> Update(Main entity);
        Task<bool> DeleteById(int id);

    }
}
