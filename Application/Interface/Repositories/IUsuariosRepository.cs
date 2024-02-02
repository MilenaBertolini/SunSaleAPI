using Domain.Responses;
using Main = Domain.Entities.Usuarios;

namespace Application.Interface.Repositories
{
    public interface IUsuariosRepository : IRepositoryBase<Main>
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
        Task<Main> VerifyLogin(string user, string pass);

        Task<Main> GetByEmail(string email, bool isVerified = false);
        Task<Main> GetByLogin(string login, bool isVerified = false);
        Task<PerfilUsuario> GetPerfil(int user);
        Task<int> QuantidadeTotal();

    }
}
