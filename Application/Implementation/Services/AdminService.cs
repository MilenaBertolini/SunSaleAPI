using Main = Domain.Entities.AdminData;
using IService = Application.Interface.Services.IAdminService;
using IRepository = Application.Interface.Repositories.IAdminRepository;
using Domain.Entities;

namespace Application.Implementation.Services
{
    public class AdminService : IService
    {
        private readonly IRepository _repository;
        public AdminService(IRepository repository)
        {
            _repository = repository;
        }


        public async Task<Main> GetAllDados()
        {
            return await _repository.GetAllDados();
        }

        public async Task<IEnumerable<Questoes>> BuscaQuestoesSolicitadasRevisao(int page, int quantity)
        {
            return await _repository.BuscaQuestoesSolicitadasRevisao(page, quantity);
        }

        public async Task<IEnumerable<Prova>> BuscaProvasSolicitadasRevisao(int page, int quantity)
        {
            return await _repository.BuscaProvasSolicitadasRevisao(page, quantity);
        }

        public async Task<IEnumerable<StringPlusInt>> BuscaUsuariosSalvoQuestoes()
        {
            return await _repository.BuscaUsuariosSalvoQuestoes();
        }

        public async Task<IEnumerable<StringPlusInt>> BuscaUsuariosVerificouQuestoes()
        {
            return await _repository.BuscaUsuariosVerificouQuestoes();
        }

        public async Task<IEnumerable<RespostasPorProva>> BuscaRespostasPorProva()
        {
            return await _repository.BuscaRespostasPorProva();
        }

        public async Task<IEnumerable<RespostasPorProva>> BuscaRespostasPorMateria()
        {
            return await _repository.BuscaRespostasPorMateria();
        }

        public void Dispose()
        {
            _repository.Dispose();
        }
    }
}
