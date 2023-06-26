using Main = Domain.Entities.LicencasSunSalePro;

namespace Application.Interface.Services
{
    public interface ILicencasSunSaleProService : IDisposable
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
        Task<Main> GetById(int id);
        Task<Main> Add(Main entity);
        Task<Main> Update(Main entity);
        Task<bool> DeleteById(int id);
        Task<Main> GetByLicenca(string licenca);

    }
}
