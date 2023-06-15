using Domain.Responses;
using Main = Domain.Entities.UsuariosCrudForms;

namespace Application.Interface.Repositories
{
    public interface IUsuariosCrudFormsRepository : IRepositoryBase<Main>
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
        Task<Main> VerifyLogin(string user, string pass);

        Task<Main> GetByEmail(string email);
        Task<Main> GetByLogin(string login);
        Task<PerfilUsuario> GetPerfil(int user);

        Task<IEnumerable<Main>> GetByUser(int id);
    }
}
