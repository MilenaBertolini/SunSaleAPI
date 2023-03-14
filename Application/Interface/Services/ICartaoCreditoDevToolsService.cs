using Main = Domain.Entities.CartaoCreditoDevTools;

namespace Application.Interface.Services
{
    public interface ICartaoCreditoDevToolsService : IDisposable
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
        Task<Main> GetByCartao(string cartao);
        Task<Main> Add(Main entity);
        Task<Main> Update(Main entity);
        Task<bool> DeleteById(int id);
        Task<IEnumerable<Main>> GetRandom(int? random);

    }
}
