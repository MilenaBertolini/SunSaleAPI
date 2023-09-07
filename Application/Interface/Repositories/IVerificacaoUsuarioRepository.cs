using Main = Domain.Entities.VerificacaoUsuario;

namespace Application.Interface.Repositories
{
    public interface IVerificacaoUsuarioRepository : IRepositoryBase<Main>
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);

        Task<Main> GetByGuid(string guid);
        Task<Main> GetByCodigoUsuario(int usuario);
    }
}
