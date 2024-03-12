using Main = Domain.Entities.Logger;

namespace Application.Interface.Repositories
{
    public interface ILoggerRepository : IRepositoryBase<Main>
    {
        Task<IEnumerable<Main>> GetAll();
        Task<Tuple<IEnumerable<Main>, int>> GetAllPagged(int page, int quantity, string message);
        Task<int> QuantidadeTotal();
    }
}
