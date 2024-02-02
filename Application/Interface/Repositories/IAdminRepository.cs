using Domain.Entities;
using Main = Domain.Entities.AdminData;

namespace Application.Interface.Repositories
{
    public interface IAdminRepository
    {
        Task<Main> GetAllDados();
        void Dispose();
        Task<IEnumerable<Questoes>> BuscaQuestoesSolicitadasRevisao(int page, int quantity);
    }
}
