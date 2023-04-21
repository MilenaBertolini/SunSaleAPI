using Application.Model;
using Main = Domain.Entities.Questoes;

namespace Application.Interface.Repositories
{
    public interface IQuestoesRepository : IRepositoryBase<Main>
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity, int? codigoProva, string? subject);
        Task<IEnumerable<string>> GetMaterias(int prova = -1);
        Task<IEnumerable<Test>> GetTests(int id = -1);
        Task<IEnumerable<Main>> GetByProva(int prova);
        Task<IEnumerable<Main>> GetByMateria(string materia);
    }
}
