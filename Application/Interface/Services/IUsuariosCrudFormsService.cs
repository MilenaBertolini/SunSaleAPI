using Domain.Responses;
using Main = Domain.Entities.UsuariosCrudForms;

namespace Application.Interface.Services
{
    public interface IUsuariosCrudFormsService : IDisposable
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
        Task<Main> GetById(int id);
        Task<Main> Add(Main entity);
        Task<Main> Update(Main entity);
        Task<bool> DeleteById(int id);
        Task<Main> GetByLogin(string user, string pass);
        Task<Main> GetByEmail(string email);
        Task<bool> ExistsEmail(string email);
        Task<bool> ExistsLogin(string login);
        Task<PerfilUsuario> GetPerfil(int user);
        Task<IEnumerable<Main>> GetByUser(int id);
    }
}
