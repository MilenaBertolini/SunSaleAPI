using Main = Domain.Entities.EmpresaForDev;

namespace Application.Interface.Repositories
{
    public interface IEmpresaForDevRepository : IRepositoryBase<Main>
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);

        Task<Main> GetByCnpj(string cpf);
    }
}
