using Main = Domain.Entities.PessoasForDev;

namespace Application.Interface.Repositories
{
    public interface IPessoasForDevRepository : IRepositoryBase<Main>
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);

        Task<Main> GetByCpf(string cpf);
        Task<IEnumerable<Main>> GetRandom(int qt);
    }
}
