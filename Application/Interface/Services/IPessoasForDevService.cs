using Main = Domain.Entities.PessoasForDev;

namespace Application.Interface.Services
{
    public interface IPessoasForDevService : IDisposable
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
        Task<Main> GetByCpf(string cpf);
        Task<Main> Add(Main entity);
        Task<Main> Update(Main entity);
        Task<bool> DeleteById(int id);
    }
}
