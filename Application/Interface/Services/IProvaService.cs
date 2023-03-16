using Main = Domain.Entities.Prova;

namespace Application.Interface.Services
{
    public interface IProvaService : IDisposable
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
        Task<Main> GetById(int id);
        Task<Main> Add(Main entity, int codigoUsuario);
        Task<Main> Update(Main entity);
        Task<bool> DeleteById(int id);

    }
}
