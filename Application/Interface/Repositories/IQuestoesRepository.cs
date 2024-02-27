using Application.Model;
using static Data.Helper.EnumeratorsTypes;
using Main = Domain.Entities.Questoes;

namespace Application.Interface.Repositories
{
    public interface IQuestoesRepository : IRepositoryBase<Main>
    {
        Task<IEnumerable<Main>> GetAll();
        Task<Tuple<IEnumerable<Main>, int>> GetAllPagged(int page, int quantity, int user, bool includeAnexos, string subject, string bancas, string provas, string materias, string tipos, int? codigoProva);
        Task<IEnumerable<string>> GetMaterias(int prova = -1);
        Task<IEnumerable<Test>> GetTests(int id = -1);
        Task<Main> GetByProva(int prova, int numero);
        Task<IEnumerable<Main>> GetByProva(int prova);
        Task<IEnumerable<Main>> GetByMateria(string materia);
        Task<int> QuantidadeQuestoes(int prova, int user = -1);
        Task<IEnumerable<string>> GetAllMateris();
        Task<Main> GetQuestoesAleatoria(TipoQuestoes tipo, string? subject, string? banca);
        Task<IEnumerable<Main>> GetQuestoesRespondidas(int usuario);
        Task<Main> GetLastByProva(int prova);
        Task<Main> UpdateAtivo(int id, bool ativo, int user);
        Task<Main> GetQuestoesByAvaliacao(int codigoAvaliacao, int numeroQuestao);
        Task<Main> UpdateAssunto(int id, string assunto, int user);
    }
}
