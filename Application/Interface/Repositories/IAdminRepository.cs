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
        Task<IEnumerable<RespostasPorProva>> BuscaRespostasPorProva(int user = -1);
        Task<IEnumerable<RespostasPorProva>> BuscaRespostasPorMateria(int user = -1);
        Task<IEnumerable<RespostasPorProva>> BuscaRespostasPorBanca(int user = -1);
        Task<IEnumerable<RespostasPorProva>> BuscaRespostasPorTipo(int user = -1);
        Task<IEnumerable<RespostasPorProva>> BuscaRespostasPorAvaliacao(int user = -1);
    }
}
