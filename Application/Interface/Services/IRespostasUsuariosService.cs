using Main = Domain.Entities.RespostasUsuarios;

namespace Application.Interface.Services
{
    public interface IRespostasUsuariosService : IDisposable
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
        Task<Main> GetById(int id);
        Task<Main> Add(Main entity);
        Task<Main> Update(Main entity);
        Task<bool> DeleteById(int id);
        Task<IEnumerable<Main>> GetByUser(int user);
        Task<IEnumerable<Main>> GetByQuestao(int questao);
        Task<IEnumerable<Main>> GetByUserQuestao(int user, int questao);

    }
}
