using Application.Model;
using Main = Domain.Entities.Questoes;

namespace Application.Interface.Services
{
    public interface IQuestoesService : IDisposable
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
        Task<Main> GetById(int id);
        Task<Main> Add(Main entity);
        Task<Main> Update(Main entity);
        Task<bool> DeleteById(int id);
        Task<IEnumerable<string>> GetMaterias(int? prova);
        Task<IEnumerable<Test>> GetTests(int? id);
        Task<IEnumerable<Main>> GetQuestoesByProva(int prova);
        Task<IEnumerable<Main>> GetQuestoesByMateria(string prova);

    }
}
