using Main = Domain.Entities.CartaoCreditoDevTools;

namespace Application.Interface.Repositories
{
    public interface ICartaoCreditoDevToolsRepository : IRepositoryBase<Main>
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
        Task<Main> GetByCartao(string cartao);
        Task<IEnumerable<Main>> GetRandom(int qt);
    }
}
