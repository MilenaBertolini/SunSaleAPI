using Main = Domain.Entities.TipoProvaAssociado;

namespace Application.Interface.Services
{
    public interface ITipoProvaAssociadoService : IDisposable
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
        Task<IEnumerable<Main>> GetAllByProva(int codigoProva);
        Task<Main> GetById(int id);
        Task<Main> Add(Main entity);
        Task<Main> Update(Main entity);
        Task<bool> DeleteById(int id);

    }
}
