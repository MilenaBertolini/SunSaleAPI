using Main = Domain.Entities.Simulados;

namespace Application.Interface.Repositories
{
    public interface ISimuladosRepository : IRepositoryBase<Main>
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity, int user);

        Task<Main> GetByProvaUser(int provaCodigo, int user);
    }
}
