using Main = Domain.Entities.VeiculosForDev;

namespace Application.Interface.Repositories
{
    public interface IVeiculosForDevRepository : IRepositoryBase<Main>
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
        Task<Main> GetByRenavam(string renavam);
        Task<IEnumerable<Main>> GetRandom(int qt);
    }
}
