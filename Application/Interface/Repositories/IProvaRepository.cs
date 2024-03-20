using Main = Domain.Entities.Prova;

namespace Application.Interface.Repositories
{
    public interface IProvaRepository : IRepositoryBase<Main>
    {
        Task<IEnumerable<Main>> GetSimulados();
        Task<Tuple<IEnumerable<Main>, int>> GetAllPagged(int page, int quantity, string bancas, string provas, string tipos, bool admin);
        Task<IEnumerable<Main>> GetAll();
        Task<bool> UpdateStatus(int id, bool active);
        Task<IEnumerable<string>> GetBancas(string provas, string materias, string assuntos, string tipos, bool admin);
        Task<IEnumerable<string>> GetProvas(string bancas, string materias, string assuntos, string tipos, bool admin);
        Task<IEnumerable<string>> GetMaterias(string bancas, string provas, string assuntos, string tipos, bool admin);
        Task<IEnumerable<string>> GetAssuntos(string bancas, string provas, string materias, string tipos, bool admin);
        Task<IEnumerable<string>> GetTipos(string bancas, string provas, string materias, bool admin);
    }
}
