using Main = Domain.Entities.Metas;

namespace Application.Interface.Repositories
{
    public interface IMetasRepository : IRepositoryBase<Main>
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
    }
}
