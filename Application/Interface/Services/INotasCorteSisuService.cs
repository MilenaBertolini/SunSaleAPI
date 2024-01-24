using Domain.Entities;
using Main = Domain.Entities.NotasCorteSisu;

namespace Application.Interface.Services
{
    public interface INotasCorteSisuService : IDisposable
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
        Task<Main> GetById(int id);
        Task<Main> Add(Main entity);
        Task<Main> Update(Main entity);
        Task<bool> DeleteById(int id);

        IEnumerable<OpcaoCodigoNome> GetInstituicoes();
        IEnumerable<OpcaoCodigoNome> GetCursosFromInstituicao(int instituicao);
        Task<List<Main>> GetByCursoAndInstituicao(long curso, int instituicao);
    }
}
