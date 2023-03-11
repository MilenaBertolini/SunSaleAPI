using Main = Domain.Entities.AcaoUsuario;

namespace Application.Interface.Repositories
{
    public interface IAcaoUsuarioRepository : IRepositoryBase<Main>
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
    }
}
