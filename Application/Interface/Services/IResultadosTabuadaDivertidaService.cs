using Domain.Responses;
using Main = Domain.Entities.ResultadosTabuadaDivertida;

namespace Application.Interface.Services
{
    public interface IResultadosTabuadaDivertidaService : IDisposable
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
        Task<Main> GetById(int id);
        Task<Main> Add(Main entity);
        Task<Main> Update(Main entity);
        Task<bool> DeleteById(int id);
        Task<List<RankingTabuadaDivertida>> GetRankingTabuada();

    }
}
