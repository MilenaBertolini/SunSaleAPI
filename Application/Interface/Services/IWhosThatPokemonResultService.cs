using Main = Domain.Entities.WhosThatPokemonResult;

namespace Application.Interface.Services
{
    public interface IWhosThatPokemonResultService : IDisposable
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetRanking(bool custom);
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
        Task<Main> GetById(int id);
        Task<Main> Add(Main entity);
        Task<Main> Update(Main entity);
        Task<bool> DeleteById(int id);

    }
}
