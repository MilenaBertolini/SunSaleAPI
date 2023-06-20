using Main = Domain.Entities.Prova;

namespace Application.Interface.Services
{
    public interface IProvaService : IDisposable
    {
        Task<IEnumerable<Main>> GetSimulados();
        Task<Tuple<IEnumerable<Main>, int>> GetAllPagged(int page, int quantity, string prova, bool admin);
        Task<IEnumerable<Main>> GetAll();
        Task<Main> GetById(int id);
        Task<Main> Add(Main entity, int codigoUsuario);
        Task<Main> Update(Main entity, int user);
        Task<bool> DeleteById(int id);

    }
}
