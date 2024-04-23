using Main = Domain.Entities.WhosThatPokemonResult;

namespace Application.Interface.Repositories
{
    public interface IWhosThatPokemonResultRepository : IRepositoryBase<Main>
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetRanking(bool custom);
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
    }
}
