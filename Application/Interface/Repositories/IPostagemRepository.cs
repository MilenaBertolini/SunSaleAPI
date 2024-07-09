using static Data.Helper.EnumeratorsTypes;
using Main = Domain.Entities.Postagem;

namespace Application.Interface.Repositories
{
    public interface IPostagemRepository : IRepositoryBase<Main>
    {
        Task<IEnumerable<Main>> GetAll();
        Task<Tuple<IEnumerable<Main>, int>> GetAllPagged(int page, int quantity, TipoPostagem tipoPostagem);
    }
}
