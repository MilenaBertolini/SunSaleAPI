using Main = Domain.Entities.QuestoesAvaliacao;

namespace Application.Interface.Services
{
    public interface IQuestoesAvaliacaoService : IDisposable
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
        Task<Main> GetById(int id);
        Task<Main> Add(Main entity, int user);
        Task<Main> Update(Main entity);
        Task<bool> DeleteById(int id);
        Task<IEnumerable<Main>> GetAllByAvaliacao(int avaliacao);
    }
}
