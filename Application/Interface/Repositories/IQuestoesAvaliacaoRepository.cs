using Main = Domain.Entities.QuestoesAvaliacao;

namespace Application.Interface.Repositories
{
    public interface IQuestoesAvaliacaoRepository : IRepositoryBase<Main>
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
        Task<IEnumerable<Main>> GetAllByAvaliacao(int avaliacao);
    }
}
