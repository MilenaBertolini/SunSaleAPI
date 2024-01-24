using Domain.Entities;
using Main = Domain.Entities.NotasCorteSisu;

namespace Application.Interface.Repositories
{
    public interface INotasCorteSisuRepository : IRepositoryBase<Main>
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
        IEnumerable<OpcaoCodigoNome> GetInstituicoes();
        IEnumerable<OpcaoCodigoNome> GetCursosFromInstituicao(int instituicao);
        Task<List<Main>> GetByCursoAndInstituicao(long curso, int instituicao);
    }
}
