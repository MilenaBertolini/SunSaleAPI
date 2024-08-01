using Main = Domain.Entities.SavedResultsWpp;

namespace Application.Interface.Repositories
{
    public interface ISavedResultsWppRepository : IRepositoryBase<Main>
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
    }
}
