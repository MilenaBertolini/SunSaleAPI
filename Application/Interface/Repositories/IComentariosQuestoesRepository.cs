using Application.Model;
using Main = Domain.Entities.ComentariosQuestoes;

namespace Application.Interface.Repositories
{
    public interface IComentariosQuestoesRepository : IRepositoryBase<Main>
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
        Task<IEnumerable<ComentariosViewModel>> GetByQuestao(int questao);
    }
}
