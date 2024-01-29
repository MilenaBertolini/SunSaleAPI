using Main = Domain.Entities.Prova;

namespace Application.Interface.Repositories
{
    public interface IProvaRepository : IRepositoryBase<Main>
    {
        Task<IEnumerable<Main>> GetSimulados();
        Task<Tuple<IEnumerable<Main>, int>> GetAllPagged(int page, int quantity, string tipo, string prova, bool admin);
        Task<IEnumerable<Main>> GetAll();
        Task<bool> UpdateStatus(int id, bool active);
    }
}
