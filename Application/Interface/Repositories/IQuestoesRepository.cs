using Application.Model;
using Main = Domain.Entities.Questoes;

namespace Application.Interface.Repositories
{
    public interface IQuestoesRepository : IRepositoryBase<Main>
    {
        Task<IEnumerable<Main>> GetAll();
        Task<Tuple<IEnumerable<Main>, int>> GetAllPagged(int page, int quantity, int? codigoProva, string? subject);
        Task<IEnumerable<string>> GetMaterias(int prova = -1);
        Task<IEnumerable<Test>> GetTests(int id = -1);
        Task<IEnumerable<Main>> GetByProva(int prova, int numero = -1);
        Task<IEnumerable<Main>> GetByMateria(string materia);
        Task<int> QuantidadeQuestoes(int prova, int user = -1);

    }
}
