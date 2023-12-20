using Main = Domain.Entities.AnexoResposta;

namespace Application.Interface.Repositories
{
    public interface IAnexoRespostaRepository : IRepositoryBase<Main>
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetByQuestaoId(int id);
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
    }
}
