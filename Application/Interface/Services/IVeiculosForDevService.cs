using Main = Domain.Entities.VeiculosForDev;

namespace Application.Interface.Services
{
    public interface IVeiculosForDevService : IDisposable
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
        Task<Main> Add(Main entity);
        Task<Main> Update(Main entity);
        Task<Main> GetByRenavam(string renavam);
        Task<IEnumerable<Main>> GetRandom(int? random);
    }
}
