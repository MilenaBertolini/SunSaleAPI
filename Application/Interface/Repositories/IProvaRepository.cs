using Main = Domain.Entities.Prova;

namespace Application.Interface.Repositories
{
    public interface IProvaRepository : IRepositoryBase<Main>
    {
        Task<IEnumerable<Main>> GetAll();
        Task<Tuple<IEnumerable<Main>, int>> GetAllPagged(int page, int quantity);
    }
}
