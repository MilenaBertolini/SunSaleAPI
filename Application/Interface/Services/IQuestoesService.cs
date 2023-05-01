using Application.Model;
using Main = Domain.Entities.Questoes;

namespace Application.Interface.Services
{
    public interface IQuestoesService : IDisposable
    {
        Task<IEnumerable<Main>> GetAll();
        Task<Tuple<IEnumerable<Main>, int>> GetAllPagged(int page, int quantity, int? codigoProva, string? subject);
        Task<Main> GetById(int id);
        Task<Main> Add(Main entity, int user);
        Task<Main> Update(Main entity, int user);
        Task<bool> DeleteById(int id);
        Task<IEnumerable<string>> GetMaterias(int? prova);
        Task<IEnumerable<Test>> GetTests(int? id);
        Task<IEnumerable<Main>> GetQuestoesByProva(int prova, int numero = -1);
        Task<IEnumerable<Main>> GetQuestoesByMateria(string prova);
        Task<int> QuantidadeQuestoes(int prova, int user = -1);
    }
}
