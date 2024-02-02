using Domain.Entities;
using Main = Domain.Entities.AdminData;

namespace Application.Interface.Services
{
    public interface IAdminService : IDisposable
    {
        Task<Main> GetAllDados();
        Task<IEnumerable<Questoes>> BuscaQuestoesSolicitadasRevisao(int page, int quantity);
    }
}
