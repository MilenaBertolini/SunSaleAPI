using Main = Domain.Entities.RecuperaSenha;

namespace Application.Interface.Repositories
{
    public interface IRecuperaSenhaRepository : IRepositoryBase<Main>
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
        Task<Main> GetByGuid(string guid);
    }
}
