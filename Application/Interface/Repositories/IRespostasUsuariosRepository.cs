using Domain.Responses;
using Main = Domain.Entities.RespostasUsuarios;

namespace Application.Interface.Repositories
{
    public interface IRespostasUsuariosRepository : IRepositoryBase<Main>
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity, int user);
        Task<IEnumerable<Main>> GetByUser(int user);
        Task<IEnumerable<Main>> GetByQuestao(int questao);
        Task<IEnumerable<Main>> GetByUserQuestao(int user, int questao);
        Task<IEnumerable<HistoricoUsuario>> GetHistory(int user);
        Task<int> GetQuantidadeQuestoesCertas(int user);
        Task<int> GetQuantidadeQuestoesTentadas(int user);
    }
}
