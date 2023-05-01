using Main = Domain.Entities.RespostasUsuarios;

namespace Application.Interface.Repositories
{
    public interface IRespostasUsuariosRepository : IRepositoryBase<Main>
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
        Task<IEnumerable<Main>> GetByUser(int user);
        Task<IEnumerable<Main>> GetByQuestao(int questao);
        Task<IEnumerable<Main>> GetByUserQuestao(int user, int questao);
    }
}
