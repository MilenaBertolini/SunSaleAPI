using Main = Domain.Entities.Alimentos;

namespace Application.Interface.Repositories
{
    public interface IAlimentosRepository : IRepositoryBase<Main>
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
    }
}
