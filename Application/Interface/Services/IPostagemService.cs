using static Data.Helper.EnumeratorsTypes;
using Main = Domain.Entities.Postagem;

namespace Application.Interface.Services
{
    public interface IPostagemService : IDisposable
    {
        Task<IEnumerable<Main>> GetAll();
        Task<Tuple<IEnumerable<Main>, int>> GetAllPagged(int page, int quantity, TipoPostagem tipoPostagem);
        Task<Main> GetById(int id);
        Task<Main> Add(Main entity);
        Task<Main> Update(Main entity);
        Task<bool> DeleteById(int id);

    }
}
