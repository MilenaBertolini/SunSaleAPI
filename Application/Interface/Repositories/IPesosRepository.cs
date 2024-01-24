using Domain.Entities;
using Main = Domain.Entities.Pesos;

namespace Application.Interface.Repositories
{
    public interface IPesosRepository : IRepositoryBase<Main>
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
        Task<IEnumerable<Main>> GetByCurso(int curso);
        List<OpcaoCodigoNome> GetPesos();
    }
}
