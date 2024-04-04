using Domain.Entities;
using Main = Domain.Entities.AdminData;

namespace Application.Interface.Repositories
{
    public interface IAdminRepository
    {
        Task<Main> GetAllDados();
        void Dispose();
        Task<IEnumerable<Questoes>> BuscaQuestoesSolicitadasRevisao(int page, int quantity);
        Task<IEnumerable<Prova>> BuscaProvasSolicitadasRevisao(int page, int quantity);
        Task<IEnumerable<StringPlusInt>> BuscaUsuariosSalvoQuestoes();
        Task<IEnumerable<StringPlusInt>> BuscaUsuariosVerificouQuestoes();
        Task<IEnumerable<RespostasPorProva>> BuscaRespostasPorProva();
        Task<IEnumerable<RespostasPorProva>> BuscaRespostasPorMateria();
    }
}
