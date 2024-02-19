using Domain.Entities;
using Main = Domain.Entities.RespostasAvaliacoes;

namespace Application.Interface.Services
{
    public interface IRespostasAvaliacoesService : IDisposable
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
        Task<Main> GetById(int id);
        Task<Main> Add(Main entity);
        Task<Main> Update(Main entity);
        Task<bool> DeleteById(int id);
        Task<IEnumerable<Main>> GetByAvaliacao(int avaliacao, int user);
        Task<IEnumerable<Usuarios>> GetUsuariosFizeramAvaliacao(int avaliacao);
    }
}
