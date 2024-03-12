using Domain.Responses;
using Main = Domain.Entities.RespostasUsuarios;

namespace Application.Interface.Services
{
    public interface IRespostasUsuariosService : IDisposable
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity, int user);
        Task<Main> GetById(int id);
        Task<Main> Add(Main entity);
        Task<Main> Update(Main entity);
        Task<bool> DeleteById(int id);
        Task<IEnumerable<Main>> GetByUser(int user);
        Task<IEnumerable<Main>> GetByQuestao(int questao);
        Task<IEnumerable<Main>> GetByUserQuestao(int user, int questao = -1);
        Task<Tuple<IEnumerable<HistoricoUsuario>, int, int>> GetHistory(int user, int page, int quantity);
        Task<int> GetQuantidadeQuestoesCertas(int user);
        Task<int> GetQuantidadeQuestoesTentadas(int user);
        Task<IEnumerable<Ranking>> GetRanking();
        Task<int> QuantidadeTotal();
    }
}
