using Main = Domain.Entities.Email;

namespace Application.Interface.Repositories
{
    public interface IEmailRepository : IRepositoryBase<Main>
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
    }
}
