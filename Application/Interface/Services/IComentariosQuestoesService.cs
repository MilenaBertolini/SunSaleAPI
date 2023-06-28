using Application.Model;
using Main = Domain.Entities.ComentariosQuestoes;

namespace Application.Interface.Services
{
    public interface IComentariosQuestoesService : IDisposable
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
        Task<Main> GetById(int id);
        Task<Main> Add(Main entity);
        Task<Main> Update(Main entity);
        Task<bool> DeleteById(int id);
        Task<IEnumerable<ComentariosViewModel>> GetByQuestao(int questao, int user);
    }
}
