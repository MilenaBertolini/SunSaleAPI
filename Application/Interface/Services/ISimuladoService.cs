using Application.Model;
using Domain.Entities;
using Main = Domain.Entities.Simulados;

namespace Application.Interface.Services
{
    public interface ISimuladoService : IDisposable
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity, int user);
        Task<Main> GetById(int id);
        Task<Main> Add(Main entity);
        Task<Main> Update(Main entity);
        Task<bool> DeleteById(int id);
        Task<Main> GetByProvaUser(int provaCodigo, int user);
        string CriaDocumentoDetalhado(IEnumerable<Questoes> questoes, Main simulado, Usuarios user, List<Simulado> questoesResolvidas);
    }
}
