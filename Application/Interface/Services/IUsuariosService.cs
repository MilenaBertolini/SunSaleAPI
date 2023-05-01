using Main = Domain.Entities.Usuarios;

namespace Application.Interface.Services
{
    public interface IUsuariosService : IDisposable
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
        Task<Main> GetById(int id);
        Task<Main> Add(Main entity);
        Task<Main> Update(Main entity);
        Task<bool> DeleteById(int id);
        Task<Main> GetByLogin(string user, string pass);

        Task<bool> ExistsEmail(string email);
        Task<bool> ExistsLogin(string login);
    }
}
