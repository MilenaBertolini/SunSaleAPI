using Main = Domain.Entities.Avaliacao;

namespace Application.Interface.Repositories
{
    public interface IAvaliacaoRepository : IRepositoryBase<Main>
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity, string chave, int user, string subject, string bancas, string provas, string materias, string professores);
        Task<IEnumerable<Main>> GetByUserId(int id);
        Task<int> QuantidadeTotal();
        Task<IEnumerable<string>> GetAllProfessores();
    }
}
