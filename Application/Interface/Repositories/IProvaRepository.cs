using Main = Domain.Entities.Prova;

namespace Application.Interface.Repositories
{
    public interface IProvaRepository : IRepositoryBase<Main>
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
    }
}
