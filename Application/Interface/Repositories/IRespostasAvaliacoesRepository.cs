using Domain.Entities;
using Main = Domain.Entities.RespostasAvaliacoes;

namespace Application.Interface.Repositories
{
    public interface IRespostasAvaliacoesRepository : IRepositoryBase<Main>
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
        Task<IEnumerable<Main>> GetByAvaliacao(int avaliacao, int user);
        Task<IEnumerable<Usuarios>> GetUsuariosFizeramAvaliacao(int avaliacao);
    }
}
