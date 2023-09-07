using Main = Domain.Entities.VerificacaoUsuario;

namespace Application.Interface.Services
{
    public interface IVerificacaoUsuarioService : IDisposable
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
        Task<Main> GetById(int id);
        Task<Main> GetByGuid(string guid);
        Task<Main> GetByCodigoUsuario(int usuario);
        Task<Main> Add(Main entity);
        Task<Main> Update(Main entity);
        Task<bool> DeleteById(int id);

    }
}
