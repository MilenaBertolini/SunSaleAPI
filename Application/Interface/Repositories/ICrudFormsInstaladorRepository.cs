using Main = Domain.Entities.CrudFormsInstalador;

namespace Application.Interface.Repositories
{
    public interface ICrudFormsInstaladorRepository : IRepositoryBase<Main>
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);

        Task<string> GetLastVerion();
        Task<Main> GetByVersao(string versao);
    }
}
