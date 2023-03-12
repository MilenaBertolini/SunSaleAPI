using Main = Domain.Entities.Codigos_Table;

namespace Application.Interface.Repositories
{
    public interface ICodigosTableRepository : IRepositoryBase<Main>
    {
        Task<int> GetNextCodigo(string table);
    }
}
