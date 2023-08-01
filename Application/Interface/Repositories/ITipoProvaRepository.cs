using Main = Domain.Entities.TipoProva;

namespace Application.Interface.Repositories
{
    public interface ITipoProvaRepository : IRepositoryBase<Main>
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
    }
}
