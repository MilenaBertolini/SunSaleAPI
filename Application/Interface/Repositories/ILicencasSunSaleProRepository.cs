using Main = Domain.Entities.LicencasSunSalePro;

namespace Application.Interface.Repositories
{
    public interface ILicencasSunSaleProRepository : IRepositoryBase<Main>
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
        Task<Main> GetByLicenca(string licenca);
    }
}
