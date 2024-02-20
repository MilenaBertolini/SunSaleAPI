using Application.Model;
using static Data.Helper.EnumeratorsTypes;
using Main = Domain.Entities.Questoes;

namespace Application.Interface.Services
{
    public interface IQuestoesService : IDisposable
    {
        Task<IEnumerable<Main>> GetAll();
        Task<Tuple<IEnumerable<Main>, int>> GetAllPagged(int page, int quantity, int user, bool includeAnexos, string subject, string bancas, string provas, string materias, int? codigoProva);
        Task<Main> GetById(int id);
        Task<Main> Add(Main entity, int user);
        Task<Main> Update(Main entity, int user);
        Task<bool> DeleteById(int id);
        Task<IEnumerable<string>> GetMaterias(int? prova);
        Task<IEnumerable<Test>> GetTests(int? id);
        Task<Main> GetQuestoesByProva(int prova, int numero);
        Task<IEnumerable<Main>> GetQuestoesByProva(int prova);
        Task<IEnumerable<Main>> GetQuestoesByMateria(string prova);
        Task<int> QuantidadeQuestoes(int prova, int user = -1);
        Task<IEnumerable<string>> GetAllMateris();
        Task<Main> GetQuestoesAleatoria(TipoQuestoes tipo, string? subject, string? banca);
        Task<IEnumerable<Main>> GetQuestoesRespondidas(int usuario);

        Task<Main> GetLastByProva(int prova);
        Task<Main> UpdateAtivo(int id, bool ativo, int user);
        Task<Main> GetQuestoesByAvaliacao(int codigoAvaliacao, int? numeroQuestao);
        Task<Main> UpdateAssunto(int id, string assunto, int user);
    }
}
