using Domain.Entities;
using Main = Domain.Entities.Pesos;

namespace Application.Interface.Services
{
    public interface IPesosService : IDisposable
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
        Task<IEnumerable<Main>> GetByCurso(int curso);
        Task<Main> GetById(int id);
        Task<Main> Add(Main entity);
        Task<Main> Update(Main entity);
        Task<bool> DeleteById(int id);
        List<OpcaoCodigoNome> GetPesos();

    }
}
