using Main = Domain.Entities.Usuarios;

namespace Application.Interface.Repositories
{
    public interface IUsuariosRepository : IRepositoryBase<Main>
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
    }
}
