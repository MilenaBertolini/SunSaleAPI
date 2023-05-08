using Domain.Responses;
using Main = Domain.Entities.ResultadosTabuadaDivertida;

namespace Application.Interface.Repositories
{
    public interface IResultadosTabuadaDivertidaRepository : IRepositoryBase<Main>
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
        Task<List<RankingTabuadaDivertida>> GetRankingTabuada();
    }
}
