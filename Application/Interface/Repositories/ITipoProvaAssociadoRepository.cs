using Main = Domain.Entities.TipoProvaAssociado;

namespace Application.Interface.Repositories
{
    public interface ITipoProvaAssociadoRepository : IRepositoryBase<Main>
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
        Task<IEnumerable<Main>> GetAllByProva(int codigoProva);
    }
}
