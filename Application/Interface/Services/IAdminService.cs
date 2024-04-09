using Domain.Entities;
using Main = Domain.Entities.AdminData;

namespace Application.Interface.Services
{
    public interface IAdminService : IDisposable
    {
        Task<Main> GetAllDados();
        Task<IEnumerable<Questoes>> BuscaQuestoesSolicitadasRevisao(int page, int quantity);
        Task<IEnumerable<Prova>> BuscaProvasSolicitadasRevisao(int page, int quantity);
        Task<IEnumerable<StringPlusInt>> BuscaUsuariosSalvoQuestoes();
        Task<IEnumerable<StringPlusInt>> BuscaUsuariosVerificouQuestoes();
        Task<IEnumerable<RespostasPorProva>> BuscaRespostasPorProva();
        Task<IEnumerable<RespostasPorProva>> BuscaRespostasPorMateria();
        Task<IEnumerable<RespostasPorProva>> BuscaRespostasPorBanca();
        Task<IEnumerable<RespostasPorProva>> BuscaRespostasPorTipo();
    }
}
