using Main = Domain.Entities.CategoriaAlimentos;

namespace Application.Interface.Repositories
{
    public interface ICategoriaAlimentosRepository : IRepositoryBase<Main>
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
    }
}
