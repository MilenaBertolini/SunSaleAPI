using Main = Domain.Entities.AnexosQuestoes;

namespace Application.Interface.Repositories
{
    public interface IAnexosQuestoesRepository : IRepositoryBase<Main>
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetByQuestaoId(int id);
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
    }
}
