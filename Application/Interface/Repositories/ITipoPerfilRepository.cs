using Main = Domain.Entities.TipoPerfil;

namespace Application.Interface.Repositories
{
    public interface ITipoPerfilRepository : IRepositoryBase<Main>
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
    }
}
