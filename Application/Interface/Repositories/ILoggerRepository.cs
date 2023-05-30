using Main = Domain.Entities.Logger;

namespace Application.Interface.Repositories
{
    public interface ILoggerRepository : IRepositoryBase<Main>
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
    }
}
