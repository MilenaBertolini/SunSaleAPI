using Main = Domain.Entities.EmpresaForDev;

namespace Application.Interface.Services
{
    public interface IEmpresaForDevService : IDisposable
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
        Task<Main> GetByCnpj(string cnpj);
        Task<Main> Add(Main entity);
        Task<Main> Update(Main entity);
        Task<bool> DeleteById(int id);
    }
}
