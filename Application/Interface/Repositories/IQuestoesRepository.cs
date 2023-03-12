using Main = Domain.Entities.Questoes;

namespace Application.Interface.Repositories
{
    public interface IQuestoesRepository : IRepositoryBase<Main>
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
    }
}
