using Main = Domain.Entities.RespostasQuestoes;

namespace Application.Interface.Repositories
{
    public interface IRespostasQuestoesRepository : IRepositoryBase<Main>
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
        Task<IEnumerable<Main>> GetByCodigoQuestao(int codigo);
    }
}
